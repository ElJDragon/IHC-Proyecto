using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIncidentes.Web.Controllers
{
    [ApiController]
    [Route("api/admin/roles")]
    [Authorize] // Protege todos los endpoints con token
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepo;
        public RolesController(IRoleRepository roleRepo) => _roleRepo = roleRepo;

        // -------------------- Listar todos los roles --------------------
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var roles = await _roleRepo.ListAsync();
            return Ok(roles);
        }

        // -------------------- Crear rol --------------------
        public record CreateRoleDto(string Name);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "El nombre del rol es requerido" });

            var role = Role.Create(dto.Name);
            await _roleRepo.AddAsync(role);

            return CreatedAtAction(nameof(List), new { id = role.Id }, role);
        }
    }
}
