using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class TipodeHabitacion
{
    public int IdtipodeHabitacion { get; set; }

    public string? NombreTipodeHabitacion { get; set; }

    public string? Descripcion { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();
}
