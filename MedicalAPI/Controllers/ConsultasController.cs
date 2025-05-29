using System.Security.Claims;
using System.Text;
using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly ConsultaService _consultaService;

        public ConsultasController(ConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        // POST: /api/consultas
        [HttpPost]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> Registrar(Consulta consulta)
        {
            consulta.MedicoId = User.FindFirstValue("id");
            consulta.FechaHora = DateTime.UtcNow;
            await _consultaService.CrearAsync(consulta);
            return Ok("Consulta registrada.");
        }

        // GET: /api/consultas/paciente/{id}
        [HttpGet("paciente/{id}")]
        [Authorize(Roles = "medico")]
        public async Task<ActionResult<List<Consulta>>> VerHistorial(string id)
        {
            var historial = await _consultaService.HistorialPorPacienteAsync(id);
            return Ok(historial);
        }

        // GET: /api/consultas/medico/{id}/fechas
        [HttpGet("reportes")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> ReportePorFechas([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var medicoId = User.FindFirstValue("id");
            var lista = await _consultaService.FiltrarPorMedicoYFechas(medicoId, desde, hasta);
            return Ok(lista);
        }

        //Exportacion CVS de consultas
        [HttpGet("export/csv")]
        [Authorize(Roles = "medico")]
        public async Task<IActionResult> ExportarCsv([FromServices] ConsultaService consultaService)
        {
            var medicoId = User.FindFirstValue("id");
            var consultas = await consultaService.HistorialPorPacienteAsync(medicoId);
            var csv = new StringBuilder();
            csv.AppendLine("PacienteId,Fecha,Diagnóstico,Tratamiento");

            foreach (var c in consultas)
            {
                csv.AppendLine($"{c.PacienteId},{c.FechaHora:u},{c.Diagnostico},{c.Tratamiento}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "consultas.csv");
        }

    }
}
