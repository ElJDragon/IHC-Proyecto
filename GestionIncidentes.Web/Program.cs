using System.Security.Claims;
using System.Text;
using GestionIncidentes.Application.Handlers;
using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Mapping;
using GestionIncidentes.Infrastructure;
using GestionIncidentes.Infrastructure.Repositories;
using GestionIncidentes.Infrastructure.Services;
using GestionIncidentes.Web.Auth;
using GestionIncidentes.Web.Features.Users.Handlers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Swagger con JWT --------------------
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionIncidentes API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa 'Bearer' seguido de tu token JWT. Ejemplo: 'Bearer abc123'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// -------------------- Services / Dependency Injection --------------------

// ✅ Blazor Components y Server
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar opciones de circuito para errores detallados
builder.Services.Configure<Microsoft.AspNetCore.Components.Server.CircuitOptions>(options =>
{
    options.DetailedErrors = builder.Environment.IsDevelopment();
});

// Controllers / API
builder.Services.AddControllers();

// Razor Pages
builder.Services.AddRazorPages();

// ✅ Autenticación en cascada para Blazor
builder.Services.AddCascadingAuthenticationState();

// Agregar soporte para sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // tiempo de expiración
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Swagger / Endpoints API
builder.Services.AddEndpointsApiExplorer();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());
});

// JWT Authentication
var key = Encoding.ASCII.GetBytes("EstaClaveTieneExactamente32Bytes!!");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role
    };
});

// MediatR - Registrar todos los assemblies con handlers y event handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ListUsersQueryHandler).Assembly,  // Application assembly
        typeof(CreateUserCommandHandler).Assembly
    );
    // Registrar behaviors
    cfg.AddOpenBehavior(typeof(GestionIncidentes.Application.Behaviors.AuditBehavior<,>));
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(DomainToDtoProfile));

// -------------------- PostgreSQL / EF Core --------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GestionIncidentesDbContext>(options =>
    options.UseNpgsql(connectionString));

// -------------------- Repositorios EF Core --------------------
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IRoleRepository, EfRoleRepository>();
builder.Services.AddScoped<IDepartmentRepository, EfDepartmentRepository>();
builder.Services.AddScoped<ITicketRepository, EfTicketRepository>();

// ✅ Nuevos repositorios para Integración C
builder.Services.AddScoped<IKnowledgeRepository, KnowledgeRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ITicketReportRepository, TicketReportRepository>();

// -------------------- Servicios --------------------
builder.Services.AddScoped<IWorkloadService, WorkloadService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ReportFormatter>();

// ✅ Servicios HTTP para comunicación Frontend-Backend
builder.Services.AddHttpClient<GestionIncidentes.Web.Services.AdminTicketService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5238");
});
builder.Services.AddHttpClient<GestionIncidentes.Web.Services.TechnicianTicketService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5238");
});
builder.Services.AddHttpClient<GestionIncidentes.Web.Services.StudentReportService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5238");
});

// Usuario actual
builder.Services.AddHttpContextAccessor();

// Registrar BlazorCurrentUser directamente
builder.Services.AddScoped<GestionIncidentes.Web.Services.BlazorCurrentUser>();

// Usar HttpContextCurrentUser para APIs y BlazorCurrentUser para componentes Blazor
builder.Services.AddScoped<ICurrentUser>(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    if (httpContextAccessor.HttpContext != null)
    {
        // Si hay HttpContext disponible (API/Razor Pages), usar HttpContextCurrentUser
        return new HttpContextCurrentUser(httpContextAccessor);
    }
    else
    {
        // Si no hay HttpContext (componentes Blazor), usar BlazorCurrentUser
        var authStateProvider = sp.GetRequiredService<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider>();
        return new GestionIncidentes.Web.Services.BlazorCurrentUser(authStateProvider, httpContextAccessor);
    }
});

// Authorization handlers & policies
builder.Services.AddScoped<IAuthorizationHandler, RoleLevelHandler>();
builder.Services.AddScoped<IAuthorizationHandler, DepartmentScopeHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminLevel", policy => policy.Requirements.Add(new RoleLevelRequirement(50)));
});

// -------------------- HttpClient para Razor Pages --------------------
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7287/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddHttpClient();

// -------------------- Build app --------------------
var app = builder.Build();

// Aplicar migraciones y seed de datos automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GestionIncidentesDbContext>();
    await db.Database.MigrateAsync();
    
    // Seed de datos de prueba
    await GestionIncidentes.Infrastructure.Data.DbSeeder.SeedAsync(db);
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

// Habilitar sesión antes de auth
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Anti-forgery para Blazor
app.UseAntiforgery();

// Map controllers
app.MapControllers();

// Map Razor Pages
app.MapRazorPages();

// ✅ Map Blazor Components
app.MapRazorComponents<GestionIncidentes.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
