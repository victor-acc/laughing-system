using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class EstadosReserva
{
    public int IdEstadoReserva { get; set; }

    public string? NombreEstadoReserva { get; set; }

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
