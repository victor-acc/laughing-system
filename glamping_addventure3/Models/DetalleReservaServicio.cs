using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class DetalleReservaServicio
{
    public int IddetalleReservaServicio { get; set; }

    public int? Idreserva { get; set; }

    public int? Cantidad { get; set; }

    public double? Precio { get; set; }

    public bool? Estado { get; set; }

    public int? Idservicio { get; set; }

    public virtual Reserva? IdreservaNavigation { get; set; }

    public virtual Servicio? IdservicioNavigation { get; set; }
}
