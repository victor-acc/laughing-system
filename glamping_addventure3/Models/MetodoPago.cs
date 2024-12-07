using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class MetodoPago
{
    public int IdMetodoPago { get; set; }

    public string? NomMetodoPago { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
