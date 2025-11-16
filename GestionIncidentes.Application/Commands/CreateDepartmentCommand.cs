using GestionIncidentes.Domain.Entities;
using MediatR;
using GestionIncidentes.Application.Interfaces;
public record CreateDepartmentCommand(string Name) : IRequest<Guid>;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Guid>
{
    private readonly IDepartmentRepository _departmentRepo;

    public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepo) => _departmentRepo = departmentRepo;

    public async Task<Guid> Handle(CreateDepartmentCommand request, CancellationToken ct)
    {
        var dept = Department.Create(request.Name);
        await _departmentRepo.AddAsync(dept); // sin ct
        return dept.Id;
    }
}

