using System.Linq;
using Lab_06_Roman_Fernandez.DTOs;
using Lab_06_Roman_Fernandez.Models;
using Lab_06_Roman_Fernandez.Repository;
using Lab_06_Roman_Fernandez.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab_06_Roman_Fernandez.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtService _tokenService;

        public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork, JwtService tokenService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        // ✅ Registrar usuario con contraseña hasheada
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Correo) || string.IsNullOrWhiteSpace(model.Clave))
                return BadRequest(new { message = "Correo y contraseña son obligatorios." });

            // Verificar si el correo ya existe en la base de datos
            var existingUsers = await _unitOfWork.Repository<Usuario>().GetByStringProperty("Correo", model.Correo);
            if (existingUsers.Any())
                return Conflict(new { message = "El correo ya está registrado." });

            // Hashear la contraseña
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Clave);

            var nuevoUsuario = new Usuario
            {
                Nombre = model.Nombre,     // Asignar el nombre
                Correo = model.Correo,     // Asignar el correo
                Clave = hashedPassword,    // Asignar la contraseña hasheada
                Rol = model.Rol            // Asignar el rol
            };

            // Guardar el usuario en la base de datos
            await _unitOfWork.Repository<Usuario>().AddAsync(nuevoUsuario);
            await _unitOfWork.Complete();

            return Ok(new { message = "Usuario registrado correctamente." });
        }

        // ✅ Iniciar sesión (login) con correo y clave
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Correo) || string.IsNullOrWhiteSpace(model.Clave))
                return BadRequest(new { message = "Correo y contraseña son obligatorios." });

            // Buscar usuario por correo
            var users = await _unitOfWork.Repository<Usuario>().GetByStringProperty("Correo", model.Correo);
            var user = users.FirstOrDefault();

            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // Verificar si la contraseña es correcta usando BCrypt
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(model.Clave, user.Clave);

            if (!isPasswordCorrect)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // Generar el token JWT
            var token = _tokenService.GenerateToken(user.Correo, user.Rol ?? "User");

            return Ok(new { token });
        }

        // ✅ Endpoint solo para administradores (requiere autorización)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult GetAdminData()
        {
            return Ok("Datos solo para administradores");
        }
    }
}
