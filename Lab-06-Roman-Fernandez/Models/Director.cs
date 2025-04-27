using System;
using System.Collections.Generic;

namespace Lab_06_Roman_Fernandez.Models;

public partial class Director
{
    public int IdDirector { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Correo { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
