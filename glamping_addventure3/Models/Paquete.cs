using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace glamping_addventure3.Models;

public partial class Paquete
{
    public int Idpaquete { get; set; }

    private string? _nombrepaquete;
    [Required(ErrorMessage = "El nombre del paquete es obligatorio.")]

    [Display(Name = "Nombre del Paquete")]
    public string? NombrePaquete
    {
        get => _nombrepaquete;
        set => _nombrepaquete = string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value.Substring(1);

    }

    [Display(Name = "Imagen")]
    public byte[]? ImagenPaquete { get; set; }

    private string? _descripcion;
    [Required(ErrorMessage = "La descripción del paquete es obligatoria.")]
    public string? Descripcion
    {
        get => _descripcion;
        set => _descripcion = string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value.Substring(1);
    }

    [Display(Name = "Habitacion")]
    [Required]
    public int? Idhabitacion { get; set; }

    [Display(Name = "Servicio")]
    [Required(ErrorMessage = "Es necesario seleccionar un servicio para crear el paquete.")]
    public int? Idservicio { get; set; }
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? Precio { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; } = new List<DetalleReservaPaquete>();

    [Display(Name = "Habitacion")]
    public virtual Habitacion? IdhabitacionNavigation { get; set; }

    [Display(Name = "Servicio")]
    public virtual Servicio? IdservicioNavigation { get; set; }

    [NotMapped] // No se mapea a la base de datos
    public string? ImagenDataURL { get; set; }
}
