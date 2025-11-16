namespace GestionIncidentes.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }

        // Método de fábrica
        public static Role Create(string name, int level = 10)
        {
            return new Role
            {
                Id = Guid.NewGuid(),
                Name = name,
                Level = level
            };
        }
    }
}
