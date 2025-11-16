using Microsoft.AspNetCore.Authorization;
using GestionIncidentes.Application.Interfaces;
namespace GestionIncidentes.Web.Auth;
public class DepartmentScopeHandler : AuthorizationHandler<DepartmentScopeRequirement, Guid>
{
    private readonly ICurrentUser _currentUser;

    public DepartmentScopeHandler(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    // resourceId es el DepartmentId del recurso que se quiere acceder
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DepartmentScopeRequirement requirement,
        Guid resourceDepartmentId)
    {
        //transformar a String el resourceDepartmentId
        String resourceDepartmentIdStr = resourceDepartmentId.ToString();

        if (_currentUser.DepartmentId == resourceDepartmentIdStr)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
