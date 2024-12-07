using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace glamping_addventure3.Models;

public partial class Servicio
{
    private string? _descripcion;
    private string? _nombreServicio;
    public int Idservicio { get; set; }
    [Required(ErrorMessage = "El nombre del servicio es obligatorio.")]

    [Display(Name = "Nombre del servicio")]
    public string? NombreServicio
    {
        get => _nombreServicio;
        set => _nombreServicio = string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value.Substring(1);
    }
    [Required(ErrorMessage = "La descripción del servicio es obligatoria.")]
    public string? Descripcion
    {
        get => _descripcion;
        set => _descripcion = string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value.Substring(1);
    }

    public string? Duracion { get; set; }
    [Required(ErrorMessage = "Ingrese una cantidad maxima valida.")]
    [Range(0, double.MaxValue, ErrorMessage = "La cantidad maxima debe ser mayor a 0.")]
    [Display(Name = "Cantidad Maxima de personas")]
    public int? CantidadMaximaPersonas { get; set; }
    [Required(ErrorMessage = "El costo de un servicio es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser un valor positivo.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El costo debe ser un número válido.")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? Costo { get; set; }
    [Required(ErrorMessage = "Confirme que el estado sera activo")]
    public bool Estado { get; set; }

    public virtual ICollection<DetalleReservaServicio> DetalleReservaServicios { get; set; } = new List<DetalleReservaServicio>();

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();

}
