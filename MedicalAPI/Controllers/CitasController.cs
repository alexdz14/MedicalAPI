using System.Security.Claims;
using MedicalAPI.DTOs;
using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly CitaService _citaService;
        private readonly LogService _logService;

        public CitasController(CitaService citaService, LogService logService)
        {
            _citaService = citaService;
            _logService = logService;
        }

        // POST: /api/citas
        [HttpPost]
        [Authorize(Roles = "recepcionista")]
        public async Task<IActionResult> Crear(CitaDto dto)
        {
            var cita = new Cita
            {
                PacienteId = dto.PacienteId,
                MedicoId = dto.MedicoId,
                FechaHora = dto.FechaHora,
                Motivo = dto.Motivo,
                Estado = "programada"
            };

            await _citaService.CrearAsync(cita);

            var userId = User.FindFirstValue("id");
            await _logService.RegistrarAsync(userId, "Creó una cita");

            return Ok("Cita registrada correctamente.");
        }

        // GET: /api/citas/paciente
        [HttpGet("paciente")]
        [Authorize(Roles = "paciente")]
        public async Task<ActionResult<List<Cita>>> GetCitasPaciente()
        {
            var pacienteId = User.FindFirstValue("id");
            var citas = await _citaService.ObtenerPorPacienteAsync(pacienteId);
            return Ok(citas);
        }

        // PUT /api/citas/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "recepcionista")]
        public async Task<IActionResult> Actualizar(string id, Cita datos)
        {
            var citaExistente = await _citaService.ObtenerPorIdAsync(id);
            if (citaExistente is null) return NotFound("Cita no encontrada.");

            datos.Id = id; // mantener ID original
            await _citaService.ActualizarAsync(id, datos);
            return Ok("Cita actualizada.");
        }

        // DELETE: /api/citas/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "recepcionista")]
        public async Task<IActionResult> Cancelar(string id)
        {
            var cita = await _citaService.ObtenerPorIdAsync(id);
            if (cita is null) return NotFound("Cita no encontrada.");

            await _citaService.CancelarAsync(id);
            return Ok("Cita cancelada.");
        }

        //Mostrar todas las citas
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<object>>> GetTodas()
        {
            var resultado = await _citaService.ObtenerTodasConNombresAsync();
            return Ok(resultado);
        }

        // GET: /api/citas/medico
        [HttpGet("medico")]
        [Authorize(Roles = "medico")]
        public async Task<ActionResult<List<CitaResumen>>> GetCitasMedico()
        {
            var medicoId = User.FindFirstValue("id");
            var citas = await _citaService.ObtenerResumenPorMedicoAsync(medicoId);
            return Ok(citas);
        }


        // GET: /api/citas/reportes
        [HttpGet("reportes")]
        [Authorize(Roles = "recepcionista")]
        public async Task<IActionResult> ReportePorEstadoYFecha([FromQuery] string estado, [FromQuery] DateTime fecha)
        {
            var citas = await _citaService.ReportePorEstadoYFecha(estado, fecha);
            return Ok(citas);
        }
    }
}
