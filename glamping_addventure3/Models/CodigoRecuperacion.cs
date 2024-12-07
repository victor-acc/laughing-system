namespace glamping_addventure3.Models
{
    public class CodigoRecuperacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }  // Relación con Usuario
        public string Codigo { get; set; }  // Código de recuperación
        public DateTime FechaExpiracion { get; set; }  // Fecha de expiración
        public bool Usado { get; set; }  // Indica si el código fue usado

        // Relación con Usuario
        public virtual Usuario Usuario { get; set; }
    }
}