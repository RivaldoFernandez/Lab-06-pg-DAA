using System;
using System.Collections.Generic;

namespace Lab_06_Roman_Fernandez.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public virtual ICollection<Director> Directors { get; set; } = new List<Director>();

    public virtual ICollection<Estudiante> Estudiantes { get; set; } = new List<Estudiante>();

    public virtual ICollection<Profesor> Profesors { get; set; } = new List<Profesor>();
}
