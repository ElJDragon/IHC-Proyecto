using GestionIncidentes.Application.Interfaces;
using GestionIncidentes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GestionIncidentes.Web.Controllers
{
    [ApiController]
    [Route("api/admin/departments")]
    [Authorize] // Todos los endpoints requieren token
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepo;

        public DepartmentsController(IDepartmentRepository departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        // -------------------- Crear departamento --------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            if (department == null || string.IsNullOrWhiteSpace(department.Name))
                return BadRequest(new { message = "Nombre del departamento requerido" });

            department.Id = Guid.NewGuid(); // asigna un ID nuevo
            await _departmentRepo.AddAsync(department);

            return CreatedAtAction(nameof(Get), new { id = department.Id }, department);
        }

        // -------------------- Listar todos los departamentos --------------------
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var departmentsList = await _departmentRepo.ListAsync();
            return Ok(departmentsList);
        }

        // -------------------- Obtener departamento por ID --------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var department = await _departmentRepo.GetByIdAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            return Ok(department);
        }

        // -------------------- DTO para actualización --------------------
        public record UpdateDepartmentDto(string Name);

        // -------------------- Actualizar departamento --------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentDto dto)
        {
            var existingDept = await _departmentRepo.GetByIdAsync(id);
            if (existingDept == null)
                return NotFound(new { message = "Department not found" });

            existingDept.Name = dto.Name;
            await _departmentRepo.UpdateAsync(existingDept);

            return NoContent();
        }

        // -------------------- Eliminar departamento --------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingDept = await _departmentRepo.GetByIdAsync(id);
            if (existingDept == null)
                return NotFound(new { message = "Department not found" });

            await _departmentRepo.DeleteAsync(id);
            return NoContent();
        }
    }
}
