using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly PacienteService _service;

        public PacientesController(PacienteService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear(Paciente paciente)
        {
            await _service.CrearAsync(paciente);
            return Ok("Paciente registrado.");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Paciente>>> GetAll() =>
            await _service.GetAllAsync();

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Actualizar(string id, [FromBody] Paciente actualizado)
        {
            var paciente = await _service.ObtenerPorIdAsync(id);
            if (paciente == null)
                return NotFound("Paciente no encontrado.");

            paciente.Nombre = actualizado.Nombre;
            paciente.Email = actualizado.Email;
            paciente.Telefono = actualizado.Telefono;

            await _service.ActualizarAsync(id, paciente);
            return Ok("Paciente actualizado.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Eliminar(string id)
        {
            var paciente = await _service.ObtenerPorIdAsync(id);
            if (paciente == null)
                return NotFound("Paciente no encontrado.");

            await _service.EliminarAsync(id);
            return Ok("Paciente eliminado.");
        }


    }
}
