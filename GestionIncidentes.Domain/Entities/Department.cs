namespace GestionIncidentes.Domain.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Opcional: relación con usuarios
        public List<User> Users { get; set; } = new();

        // -------------------- Método factory --------------------
        public static Department Create(string name)
        {
            return new Department
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }
    }
}
