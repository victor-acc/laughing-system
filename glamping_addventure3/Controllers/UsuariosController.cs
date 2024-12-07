using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Glamping_Addventure.Models.Recursos;
using Glamping_Addventure.Models.Servicios.Implementación;
using Glamping_Addventure3.Models.Servicios.Contrato;
using glamping_addventure3.Models;

namespace glamping_addventure3.Controllers
{
    [Authorize]

    public class UsuariosController : Controller
    {
        private readonly GlampingAddventure3Context _context;
        private readonly IUsuarioService _usuarioServicio;

        public UsuariosController(GlampingAddventure3Context context, IUsuarioService usuarioServicio)
        {
            _context = context;
            _usuarioServicio = usuarioServicio;
        }


        // GET: Usuarios
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.Include(u => u.IdrolNavigation).ToList();
            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Roles.ToList();
            return View(); // Este método cargará la vista con los roles disponibles.
        }

        // POST: Usuarios/Create
        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreUsuario,Email,Apellido,TipoDocumento,NumeroDocumento,Direccion,Telefono,Idrol,Contrasena")] Usuario usuario, int Idrol, string confirmarContrasena)
        {
            Console.WriteLine($"Idrol recibido: {usuario.Idrol}");
            // Validar que se seleccione un rol válido
            if (Idrol <= 0 || !_context.Roles.Any(r => r.Idrol == Idrol))
            {
                ModelState.AddModelError("Idrol", "Debe seleccionar un rol válido.");
            }

            // Validar contraseñas
            if (string.IsNullOrWhiteSpace(confirmarContrasena) || usuario.Contrasena != confirmarContrasena)
            {
                ModelState.AddModelError("Contrasena", "Las contraseñas no coinciden.");
            }

            // Validar correos duplicados
            if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
            }

            // Validar documentos duplicados
            if (_context.Usuarios.Any(u => u.NumeroDocumento == usuario.NumeroDocumento))
            {
                ModelState.AddModelError("NumeroDocumento", "El número de documento ya está registrado.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.Where(r => r.IsActive).ToList();
                return View(usuario);
            }

            usuario.Contrasena = Utilidades.EncriptarClave(usuario.Contrasena);
            usuario.Idrol = Idrol;

            Usuario usuario_creado = await _usuarioServicio.SaveUsuario(usuario);
            if (usuario_creado != null && usuario_creado.Idusuario > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "No se pudo crear el usuario.");
            ViewBag.Roles = _context.Roles.Where(r => r.IsActive).ToList();
            return View(usuario);
        }



        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        // GET: Usuarios/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewBag.Roles = _context.Roles.ToList();
            return View(usuario);

            // Verificar si es el usuario protegido
            if (usuario.Email == "Isabela.zg29@gmail.com")
            {
                TempData["Error"] = "No puedes editar este usuario.";
                return RedirectToAction("Index");
            }

            ViewBag.Roles = _context.Roles.ToList();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Idusuario,NombreUsuario,Email,Apellido,TipoDocumento,NumeroDocumento,Direccion,Telefono,Idrol")] Usuario usuario)
        {
            if (id != usuario.Idusuario)
            {
                return NotFound();
            }

            var usuarioExistente = _context.Usuarios.Find(id);

            // Verificar si es el usuario protegido
            if (usuarioExistente.Email == "isabelazg29@gmail.com")
            {
                TempData["Error"] = "No puedes editar este usuario.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {

                if (usuarioExistente != null)
                {
                    usuarioExistente.NombreUsuario = usuario.NombreUsuario;
                    usuarioExistente.Email = usuario.Email;
                    usuarioExistente.Apellido = usuario.Apellido;
                    usuarioExistente.TipoDocumento = usuario.TipoDocumento;
                    usuarioExistente.NumeroDocumento = usuario.NumeroDocumento;
                    usuarioExistente.Direccion = usuario.Direccion;
                    usuarioExistente.Telefono = usuario.Telefono;
                    usuarioExistente.Idrol = usuario.Idrol;

                    _context.Update(usuarioExistente);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Roles = _context.Roles.ToList();
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("El ID proporcionado es nulo.");
            }

            var usuario = _context.Usuarios.FirstOrDefault(m => m.Idusuario == id);
            if (usuario == null)
            {
                return NotFound("El usuario no existe en la base de datos.");
            }

            return View(usuario);

            if (usuario.Email == "isabela.zg29@gmail.com")
            {
                TempData["Error"] = "No puedes eliminar este usuario.";
                return RedirectToAction("Index");
            }

            return View(usuario);
        }
    

        // POST: Usuarios/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound("El usuario que intentas eliminar no existe.");
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

            // Verificar si es el usuario protegido
            if (usuario.Email == "isabela.zg29@gmail.com")
            {
                TempData["Error"] = "No puedes eliminar este usuario.";
                return RedirectToAction("Index");
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



    }
}
