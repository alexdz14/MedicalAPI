using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalAPI.Services;

namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly CitaService _citaService;
        private readonly ConsultaService _consultaService;

        public DashboardController(CitaService citaService, ConsultaService consultaService)
        {
            _citaService = citaService;
            _consultaService = consultaService;
        }

        [HttpGet("resumen")]
        [Authorize(Roles = "recepcionista")]
        public async Task<IActionResult> GetResumen()
        {
            var totalCitas = (await _citaService.ReportePorEstadoYFecha("programada", DateTime.UtcNow)).Count;
            var totalCanceladas = (await _citaService.ReportePorEstadoYFecha("cancelada", DateTime.UtcNow)).Count;

            return Ok(new
            {
                TotalCitasHoy = totalCitas,
                TotalCanceladasHoy = totalCanceladas
            });
        }
    }
}
