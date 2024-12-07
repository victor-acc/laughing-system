using System.Linq;
using System.Threading.Tasks;
using Glamping_Addventure3.Models;
using Glamping_Addventure3.Models.Servicios.Contrato;
using glamping_addventure3.Models;
using Microsoft.EntityFrameworkCore;

namespace Glamping_Addventure.Models.Servicios.Implementación
{
    public class UsuarioService : IUsuarioService
    {
        private readonly GlampingAddventure3Context _dbContext;

        public UsuarioService(GlampingAddventure3Context dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Role>> GetRolesActivos()
        {
            return await _dbContext.Roles
                                   .Where(r => r.IsActive)
                                   .ToListAsync();
        }

        public async Task<Usuario> GetUsuario(string email, string contrasena)
        {
            Usuario usuario_encontrado = await _dbContext.Usuarios
                .Where(u => u.Email == email && u.Contrasena == contrasena)
                .FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            _dbContext.Usuarios.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<Usuario> GetUsuarioPorEmail(string email)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<List<Usuario>> GetUsuarios(int pageNumber, int pageSize)
        {
            return await _dbContext.Usuarios
                .Include(u => u.IdrolNavigation)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Role> GetRolPorId(int Idrol)
        {
            // Lógica para obtener el rol por su ID
            var rol = await _dbContext.Roles.FindAsync(Idrol);  // Suponiendo que usas Entity Framework
            return rol;
        }

        public async Task<int> GetTotalUsuarios()
        {
            return await _dbContext.Usuarios.CountAsync();
        }

        public async Task<Usuario> GetUsuarioPorDocumento(int? numeroDocumento)
        {
            return await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.NumeroDocumento == numeroDocumento);
        }
    }
}
