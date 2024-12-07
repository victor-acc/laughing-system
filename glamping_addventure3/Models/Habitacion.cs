using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace glamping_addventure3.Models;

public partial class Habitacion
{
    public int Idhabitacion { get; set; }
    private string? _descripcion;
    private string? _nombreHabitacion;
    [Required(ErrorMessage = "El nombre de la habitación es obligatorio.")]

    [Display(Name = "Nombre de la habitación")]
    public string? NombreHabitacion
    {
        get => _nombreHabitacion;
        set => _nombreHabitacion = string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value.Substring(1);
    }
    [Display(Name = "Imagen de la habitacion")]
    public byte[]? ImagenHabitacion { get; set; }
    [Required(ErrorMessage = "La descripción de la habitación es obligatoria.")]
    public string? Descripcion
    {

        get => _descripcion;
        set => _descripcion = string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value.Substring(1);
    }

    [Required(ErrorMessage = "El costo de una habitacion es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser un valor positivo.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El costo debe ser un número válido.")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? Costo { get; set; }
    [Required(ErrorMessage = "Ingrese un tipo de habitacion valido.")]
    [Display(Name = "Tipo de habitacion")]
    public int? IdtipodeHabitacion { get; set; }
    [Required(ErrorMessage = "Confirme que el estado sera activo")]
    public bool Estado { get; set; }

    public virtual ICollection<HabitacionMueble> HabitacionMuebles { get; set; } = new List<HabitacionMueble>();

    public virtual TipodeHabitacion? IdtipodeHabitacionNavigation { get; set; }

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();
    [NotMapped] // No se mapea a la base de datos
    public string? ImagenDataURL { get; set; }
    [NotMapped]
    public string? NombreTipoHabitacion => IdtipodeHabitacionNavigation?.NombreTipodeHabitacion;
}
