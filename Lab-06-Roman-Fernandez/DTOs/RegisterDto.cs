namespace Lab_06_Roman_Fernandez.DTOs;

public class RegisterDto
{
    public string Nombre { get; set; } = null!;  // Nombre del usuario
    public string Correo { get; set; } = null!;  // Correo (será el "Username" en login)
    public string Clave { get; set; } = null!;   // Contraseña
    public string Rol { get; set; } = null!;     // Rol (Admin, User, etc.)
}
