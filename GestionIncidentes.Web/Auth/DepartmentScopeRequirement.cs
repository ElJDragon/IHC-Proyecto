using Microsoft.AspNetCore.Authorization;

namespace GestionIncidentes.Web.Auth;
public class DepartmentScopeRequirement : IAuthorizationRequirement
{
    // opcionalmente podrías pasar reglas aquí
    public DepartmentScopeRequirement() { }
}
