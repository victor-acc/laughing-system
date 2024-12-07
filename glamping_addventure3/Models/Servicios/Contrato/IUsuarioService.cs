using System.Threading.Tasks;
using glamping_addventure3.Models;

namespace Glamping_Addventure3.Models.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string email, string contrasena);

        Task<Usuario> SaveUsuario(Usuario modelo);

        Task<Usuario> GetUsuarioPorEmail(string email);

        Task<Usuario> GetUsuarioPorDocumento(int? numeroDocumento);

        Task<Role> GetRolPorId(int Idrol);

        Task<List<Role>> GetRolesActivos();

    }
}