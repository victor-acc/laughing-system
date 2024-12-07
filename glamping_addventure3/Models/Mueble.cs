using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace glamping_addventure3.Models;

public partial class Mueble
{
    private string? _nombreMueble;
    public int Idmueble { get; set; }
    [Required(ErrorMessage = "El nombre del mueble es obligatorio.")]
   
    [Display(Name = "Nombre del mueble")]
    public string? NombreMueble
    {
        get => _nombreMueble;
        set => _nombreMueble = string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value.Substring(1);
    }
    [Display(Name = "Imagen del mueble")]
    public byte[]? ImagenMueble { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "La cantidad debe ser un valor positivo.")]
    [Required(ErrorMessage = "Es necesario ingresar una cantidad disponible.")]
    [Display(Name = "Cantidad Disponible")]
    public int CantidadDisponible { get; set; }
    [Required(ErrorMessage = "El valor de un mueble es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El valor debe ser un valor positivo.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El valor debe ser un número válido.")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
    public double? Valor { get; set; }
    [Required(ErrorMessage = "Confirme que el estado sera activo")]
    public bool Estado { get; set; }

    public virtual ICollection<HabitacionMueble> HabitacionMuebles { get; set; } = new List<HabitacionMueble>();
    [NotMapped] // No se mapea a la base de datos
    public string? ImagenDataURL { get; set; }
}
