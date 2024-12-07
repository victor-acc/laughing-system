
namespace glamping_addventure3.Models
{
    public partial class TokenRecuperacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; }
    }
}