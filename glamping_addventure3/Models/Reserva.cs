using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace glamping_addventure3.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public string? NroDocumentoCliente { get; set; }

    public DateTime? FechaReserva { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinalizacion { get; set; }

    public double? SubTotal { get; set; }

    public double? Descuento { get; set; }

    public double? Iva { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? MontoTotal { get; set; }

    public int? MetodoPago { get; set; }

    public int? IdEstadoReserva { get; set; }

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    public virtual EstadosReserva? IdEstadoReservaNavigation { get; set; }

    public virtual MetodoPago? MetodoPagoNavigation { get; set; }

    public virtual Cliente? NroDocumentoClienteNavigation { get; set; }
}
