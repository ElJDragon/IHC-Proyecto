using GestionIncidentes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionIncidentes.Infrastructure
{
    public class GestionIncidentesDbContext : DbContext
    {
        public GestionIncidentesDbContext(DbContextOptions<GestionIncidentesDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Incident> Incidents => Set<Incident>(); // ✅ Integrante A
        
        // ✅ Nuevas entidades para Base de Conocimiento, Reportes, Notificaciones y Auditoría
        public DbSet<Solution> Solutions => Set<Solution>();
        public DbSet<KnowledgeEntry> KnowledgeEntries => Set<KnowledgeEntry>();
        public DbSet<SolutionStep> SolutionSteps => Set<SolutionStep>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<TicketReport> TicketReports => Set<TicketReport>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            // Suprimir advertencia de cambios pendientes en el modelo
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------- Claves primarias --------------------
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Department>().HasKey(d => d.Id);
            modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
            modelBuilder.Entity<Incident>().HasKey(i => i.Id); // ✅ Integrante A
            modelBuilder.Entity<Solution>().HasKey(s => s.Id);
            modelBuilder.Entity<KnowledgeEntry>().HasKey(k => k.Id);
            modelBuilder.Entity<SolutionStep>().HasKey(s => s.Id);
            modelBuilder.Entity<Notification>().HasKey(n => n.Id);
            modelBuilder.Entity<AuditLog>().HasKey(a => a.Id);
            modelBuilder.Entity<TicketReport>().HasKey(r => r.Id);

            // -------------------- Relaciones --------------------
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Users)
                .WithOne()
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> Incident (opcional para mantener compatibilidad con tickets legacy)
            modelBuilder.Entity<Ticket>()
                .HasOne<Incident>()
                .WithMany()
                .HasForeignKey(t => t.IncidentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); // Nullable para tickets legacy

            modelBuilder.Entity<Ticket>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ✅ Integrante A: Incident reportado por usuario
            modelBuilder.Entity<Incident>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(i => i.ReportedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------- Flujo de Soluciones --------------------
            // Ticket -> Solutions (1 a muchos)
            modelBuilder.Entity<Solution>()
                .HasOne(s => s.Ticket)
                .WithMany(t => t.Solutions)
                .HasForeignKey(s => s.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            // Solution -> CreatedBy (User)
            modelBuilder.Entity<Solution>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Solution -> SolutionSteps (1 a muchos)
            modelBuilder.Entity<SolutionStep>()
                .HasOne(ss => ss.Solution)
                .WithMany(s => s.Steps)
                .HasForeignKey(ss => ss.SolutionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Solution -> KnowledgeEntry (1 a 1 opcional)
            modelBuilder.Entity<KnowledgeEntry>()
                .HasOne(k => k.Solution)
                .WithOne(s => s.KnowledgeEntry)
                .HasForeignKey<KnowledgeEntry>(k => k.SolutionId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<KnowledgeEntry>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(k => k.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuditLog>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketReport>()
                .HasOne<Ticket>()
                .WithMany()
                .HasForeignKey(r => r.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TicketReport>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------- Propiedades requeridas --------------------
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Role).IsRequired();
            modelBuilder.Entity<Department>().Property(d => d.Name).IsRequired();
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.Title).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.Status).IsRequired();
            
            // ✅ Integrante A: Incident
            modelBuilder.Entity<Incident>().Property(i => i.Title).IsRequired();
            modelBuilder.Entity<Incident>().Property(i => i.Description).IsRequired();
            modelBuilder.Entity<Incident>().Property(i => i.Status).IsRequired();
            
            modelBuilder.Entity<Solution>().Property(s => s.Title).IsRequired();
            modelBuilder.Entity<Solution>().Property(s => s.Description).IsRequired();
            modelBuilder.Entity<Solution>().Property(s => s.Status).IsRequired();
            
            modelBuilder.Entity<KnowledgeEntry>().Property(k => k.Title).IsRequired();
            modelBuilder.Entity<KnowledgeEntry>().Property(k => k.Problem).IsRequired();
            modelBuilder.Entity<KnowledgeEntry>().Property(k => k.SolutionDescription).IsRequired();
            modelBuilder.Entity<KnowledgeEntry>().Property(k => k.Category).IsRequired();
            
            modelBuilder.Entity<SolutionStep>().Property(s => s.Title).IsRequired();
            modelBuilder.Entity<SolutionStep>().Property(s => s.Description).IsRequired();
            
            modelBuilder.Entity<Notification>().Property(n => n.Title).IsRequired();
            modelBuilder.Entity<Notification>().Property(n => n.Message).IsRequired();
            modelBuilder.Entity<Notification>().Property(n => n.Type).IsRequired();
            
            modelBuilder.Entity<AuditLog>().Property(a => a.Action).IsRequired();
            modelBuilder.Entity<AuditLog>().Property(a => a.EntityType).IsRequired();
            
            modelBuilder.Entity<TicketReport>().Property(r => r.ResolutionDetails).IsRequired();

            // -------------------- Índices para mejor performance --------------------
            modelBuilder.Entity<Notification>()
                .HasIndex(n => new { n.UserId, n.IsRead });

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => new { a.UserId, a.Timestamp });

            modelBuilder.Entity<KnowledgeEntry>()
                .HasIndex(k => k.Category);

            modelBuilder.Entity<SolutionStep>()
                .HasIndex(s => new { s.SolutionId, s.StepNumber });
        }
    }
}
