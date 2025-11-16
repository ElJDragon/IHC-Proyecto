using Microsoft.AspNetCore.Authorization;
using GestionIncidentes.Application.Interfaces;
namespace GestionIncidentes.Web.Auth;
public class RoleLevelRequirement : IAuthorizationRequirement
{
    public int MinLevel { get; }
    public RoleLevelRequirement(int minLevel)
    {
        MinLevel = minLevel;
    }
}

// Handler que valida RoleLevelRequirement
public class RoleLevelHandler : AuthorizationHandler<RoleLevelRequirement>
{
    private readonly ICurrentUser _currentUser;

    public RoleLevelHandler(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoleLevelRequirement requirement)
    {
        if (_currentUser.RoleLevel >= requirement.MinLevel)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}