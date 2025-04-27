using System;
using System.Collections.Generic;

namespace Lab_06_Roman_Fernandez.Models;

public partial class Nota
{
    public int IdNota { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdMateria { get; set; }

    public decimal Nota1 { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual Materium? IdMateriaNavigation { get; set; }
}
