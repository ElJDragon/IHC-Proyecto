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

// Controllers / API
builder.Services.AddControllers();

// Razor Pages
builder.Services.AddRazorPages();

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

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        typeof(ListUsersQueryHandler).Assembly,
        typeof(CreateUserCommandHandler).Assembly
    );
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(DomainToDtoProfile));

// -------------------- PostgreSQL / EF Core --------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GestionIncidentesDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositorios EF Core
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IRoleRepository, EfRoleRepository>();
builder.Services.AddScoped<IDepartmentRepository, EfDepartmentRepository>();
builder.Services.AddScoped<ITicketRepository, EfTicketRepository>();

// Servicios
builder.Services.AddScoped<IWorkloadService, WorkloadService>();

// Usuario actual
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpContextCurrentUser>();

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

// Aplicar migraciones automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GestionIncidentesDbContext>();
    db.Database.Migrate();
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

// Map controllers
app.MapControllers();

// Map Razor Pages
app.MapRazorPages();

// Fallback para SPA o index
app.MapFallbackToFile("index.html");

app.Run();
