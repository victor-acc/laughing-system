using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace glamping_addventure3.Models;

public partial class Abono
{
    public int Idabono { get; set; }

    public int? Idreserva { get; set; }

    public DateOnly? FechaAbono { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? ValorDeuda { get; set; }
    [DisplayFormat(DataFormatString = "{0:N1} %", ApplyFormatInEditMode = true)]
    public double? Porcentaje { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? Pendiente { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? CantAbono { get; set; }

    public byte[]? Comprobante { get; set; }

    public bool Estado { get; set; }

    public virtual Reserva? IdreservaNavigation { get; set; }
}
