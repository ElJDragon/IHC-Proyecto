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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------- Claves primarias --------------------
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Department>().HasKey(d => d.Id);
            modelBuilder.Entity<Ticket>().HasKey(t => t.Id);

            // -------------------- Relaciones --------------------
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Users)    // Un departamento tiene muchos usuarios
                .WithOne()                // Usuario pertenece a un departamento
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne<User>()           // Ticket pertenece a un usuario
                .WithMany()               // Usuario puede tener muchos tickets
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------- Propiedades requeridas --------------------
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Role).IsRequired();
            modelBuilder.Entity<Department>().Property(d => d.Name).IsRequired();
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.Title).IsRequired();
            modelBuilder.Entity<Ticket>().Property(t => t.Status).IsRequired();
        }
    }
}
