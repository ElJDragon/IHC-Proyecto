using Application.Features.Users.GetUser;
using GestionIncidentes.Application.Commands;
using GestionIncidentes.Application.Features.Users.Queries;
using GestionIncidentes.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIncidentes.Web.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize] // Protege todos los endpoints con token

    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) => _mediator = mediator;

        // -------------------- Crear usuario --------------------
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            try
            {
                var id = await _mediator.Send(
                    new CreateUserCommand(dto.Email,dto.Password, dto.FullName, dto.DepartmentId, dto.Role)
                );
                return CreatedAtAction(nameof(Get), new { id }, new { id });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("departamento"))
            {
                // Departamento no existe
                return StatusCode(403, new { message = "Departamento no existe" });
            }
        }

        // -------------------- Obtener usuario por ID --------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var user = await _mediator.Send(new GetUserQuery(id));

                if (user == null)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // -------------------- Listar todos los usuarios --------------------
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _mediator.Send(new ListUsersQuery());
            return Ok(users);
        }

        // -------------------- Listar usuarios por departamento --------------------
        [HttpGet("department/{deptId}")]
        public async Task<IActionResult> ListByDepartment(Guid deptId)
        {
            var users = await _mediator.Send(new ListUsersByDepartmentQuery(deptId));
            return Ok(users);
        }

        // -------------------- Actualizar usuario --------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
        {
            try
            {
                await _mediator.Send(new UpdateUserCommand(id, dto.FullName, dto.DepartmentId, dto.Role));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("departamento"))
            {
                return StatusCode(403, new { message = "Departamento no existe" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -------------------- Eliminar usuario --------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
