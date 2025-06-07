using System.Security.Claims;
using MedicalAPI.DTOs;
using MedicalAPI.Helpers;
using MedicalAPI.Models;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MedicalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Usuario>>> Get() =>
            await _usuarioService.GetAllAsync();


        [HttpPost("registro")]
        public async Task<IActionResult> Registrar(RegistroUsuarioDto dto)
        {
            var existe = await _usuarioService.BuscarPorEmailAsync(dto.Email);
            if (existe is not null)
                return BadRequest("Ya existe un usuario con ese correo.");

            var nuevo = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Rol = dto.Rol,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                Activo = true
            };

            await _usuarioService.CrearAsync(nuevo);
            return Ok("Usuario registrado correctamente.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            var usuario = await _usuarioService.BuscarPorEmailAsync(dto.Email);
            if (usuario == null || !PasswordHelper.VerifyPassword(dto.Password, usuario.PasswordHash))
                return Unauthorized("Credenciales inválidas.");

            var token = JwtHelper.GenerateToken(usuario.Id, usuario.Rol);
            return Ok(new
            {
                token,
                usuario = new { usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol }
            });
        }

        [HttpPut("perfil")]
        [Authorize]
        public async Task<IActionResult> EditarPerfil(EditarPerfilDto dto)
        {
            var userId = User.FindFirstValue("id");
            var usuario = await _usuarioService.ObtenerPorIdAsync(userId);

            if (usuario == null) return NotFound("Usuario no encontrado.");

            await _usuarioService.ActualizarPerfilAsync(userId, dto.Nombre, dto.Email, dto.NuevaPassword);
            return Ok("Perfil actualizado.");
        }

        [HttpGet("medicos")]
        [Authorize(Roles = "recepcionista,admin")]
        public async Task<IActionResult> GetMedicos()
        {
            var medicos = await _usuarioService.ObtenerPorRolAsync("medico");

            var resultado = medicos.Select(m => new
            {
                id = m.Id,
                nombre = m.Nombre
            });

            return Ok(resultado);
        }

        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok("pong");
        }


        [HttpGet("logs")]
        [Authorize]
        public async Task<IActionResult> MisLogs([FromServices] LogService logService)
        {
            var userId = User.FindFirstValue("id");
            var logs = await logService.ObtenerPorUsuario(userId);
            return Ok(logs);
        }

    }
}
