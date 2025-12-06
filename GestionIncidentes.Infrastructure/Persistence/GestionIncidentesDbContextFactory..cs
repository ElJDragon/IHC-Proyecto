using GestionIncidentes.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GestionIncidentes.Infrastructure
{
    public class GestionIncidentesDbContextFactory : IDesignTimeDbContextFactory<GestionIncidentesDbContext>
    {
        public GestionIncidentesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GestionIncidentesDbContext>();

            // Aquí la misma cadena de conexión que pusiste en appsettings.json
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=GestionIncidentesDb;Username=postgres;Password=12345");

            return new GestionIncidentesDbContext(optionsBuilder.Options);
        }
    }
}
