using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace GestionIncidentes.Web.Pages.Admin
{
    public class ManageModelUI : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ManageModelUI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();

        [BindProperty]
        public DepartmentDto SelectedDepartment { get; set; }

        public bool IsEditing { get; set; } = false;

        // -------------------- Cargar p�gina --------------------
        public async Task OnGetAsync(Guid? id)
        {
            await LoadDepartmentsAsync();

            if (id.HasValue)
            {
                SelectedDepartment = Departments.FirstOrDefault(d => d.Id == id.Value);
                IsEditing = SelectedDepartment != null;
            }
            else
            {
                SelectedDepartment = new DepartmentDto();
            }
        }

        // -------------------- Crear --------------------
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync();
                return Page();
            }

            var client = CreateHttpClient();
            var json = JsonSerializer.Serialize(new { Name = SelectedDepartment.Name });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/admin/departments", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al crear departamento");
                await LoadDepartmentsAsync();
                return Page();
            }

            return RedirectToPage();
        }

        // -------------------- Editar --------------------
        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync();
                return Page();
            }

            var client = CreateHttpClient();
            var json = JsonSerializer.Serialize(new { Name = SelectedDepartment.Name });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/admin/departments/{SelectedDepartment.Id}", content);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar departamento");
                await LoadDepartmentsAsync();
                return Page();
            }

            return RedirectToPage();
        }

        // -------------------- Eliminar --------------------
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "ID inv�lido");
                await LoadDepartmentsAsync();
                return Page();
            }

            var client = CreateHttpClient();
            var response = await client.DeleteAsync($"api/admin/departments/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, $"Error al eliminar departamento. C�digo: {response.StatusCode}");
            }

            return RedirectToPage();
        }

        // -------------------- Helpers --------------------
        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            // Token temporal de ejemplo
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzYzMjU0NjkzLCJleHAiOjE3NjMyNTgyOTMsImlhdCI6MTc2MzI1NDY5M30.yFWmudz6XOKS3F2Gf_H1hJFBUoK5MjP3rw681V_n9XU";
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        private async Task LoadDepartmentsAsync()
        {
            try
            {
                var client = CreateHttpClient();
                var response = await client.GetAsync("api/admin/departments");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Departments = JsonSerializer.Deserialize<List<DepartmentDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<DepartmentDto>();
                }
                else
                {
                    Departments = new List<DepartmentDto>();
                    ModelState.AddModelError(string.Empty, "No se pudieron cargar los departamentos desde la API");
                }
            }
            catch (Exception)
            {
                // API offline / conexión rechazada: mostrar página sin datos en lugar de romper
                Departments = new List<DepartmentDto>();
                ModelState.AddModelError(string.Empty, "API de administración no disponible");
            }
        }
    }

    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
