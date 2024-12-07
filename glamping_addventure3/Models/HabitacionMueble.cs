using System;
using System.Collections.Generic;

namespace glamping_addventure3.Models;

public partial class HabitacionMueble
{
    public int IdhabitacionMuebles { get; set; }

    public int? Idhabitacion { get; set; }

    public int? Idmueble { get; set; }

    public int CantidadUsada { get; set; }

    public virtual Habitacion? IdhabitacionNavigation { get; set; }

    public virtual Mueble? IdmuebleNavigation { get; set; }
}
