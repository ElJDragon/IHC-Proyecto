public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;  // ✅ nueva
    public string FullName { get; private set; } = string.Empty;
    public Guid DepartmentId { get; private set; }
    public int Workload { get; private set; }
    public string Role { get; private set; } = string.Empty;

    private User() { } // para EF

    public static User Create(string email, string password, string fullName, Guid departmentId, string role)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email requerido");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password requerido");
        if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Rol requerido");

        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = password, // ✅ asignar contraseña
            FullName = fullName,
            DepartmentId = departmentId,
            Role = role
        };
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password requerido");
        Password = password;
    }

    public void SetFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName requerido");
        FullName = fullName;
    }

    public void SetDepartment(Guid deptId)
    {
        DepartmentId = deptId;
    }

    public void SetRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Rol no puede estar vacío");
        Role = role;
    }

    public void SetWorkload(int load)
    {
        if (load < 0) throw new ArgumentOutOfRangeException(nameof(load));
        Workload = load;
    }
}

