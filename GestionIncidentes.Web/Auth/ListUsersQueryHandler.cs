using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Application.Features.Users.Queries;
using GestionIncidentes.Application.Models;
using MediatR;
using AutoMapper;
namespace GestionIncidentes.Web.Features.Users.Handlers;
public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public ListUsersQueryHandler(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(ListUsersQuery request, CancellationToken ct)
    {
        var users = await _userRepo.ListAllAsync(ct);
        return users.Select(u => _mapper.Map<UserDto>(u)).ToList();
    }
}
