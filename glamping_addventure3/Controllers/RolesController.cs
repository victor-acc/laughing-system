using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using glamping_addventure3.Models;

namespace glamping_addventure3.Controllers
{
    [Authorize]

    public class RolesController : Controller
    {
        private readonly GlampingAddventure3Context _context;

        public RolesController(GlampingAddventure3Context context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var roles = _context.Roles.Include(r => r.RolesPermisos).ToList();
            return View(roles);
        }


        [HttpPost]
        [Route("Roles/ToggleActive")]
        public IActionResult ToggleActive(int id)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(r => r.Idrol == id);
                if (role != null)
                {
                    role.IsActive = !role.IsActive;
                    _context.Update(role);
                    _context.SaveChanges();
                    return Json(new { success = true, estado = role.IsActive });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cambiar el estado: " + ex.Message });
            }
        }



        public IActionResult Create()
        {
            ViewBag.Permisos = _context.Permisos.ToList();
            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role, int[] selectedPermisos, bool isActive)
        {
            if (_context.Roles.Any(r => r.Nombre == role.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe un rol con este nombre.");
            }

            if (selectedPermisos == null || !selectedPermisos.Any())
            {
                ModelState.AddModelError("Permisos", "Debe seleccionar al menos un permiso.");
            }

            if (ModelState.IsValid)
            {
                role.IsActive = isActive;
                _context.Roles.Add(role);
                _context.SaveChanges();

                foreach (var permisoId in selectedPermisos)
                {
                    var rolPermiso = new RolesPermiso
                    {
                        Idrol = role.Idrol,
                        Idpermiso = permisoId
                    };
                    _context.RolesPermisos.Add(rolPermiso);
                }
                _context.SaveChanges();

                TempData["Success"] = "Rol creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Permisos = _context.Permisos.ToList();
            return View(role);
        }


        public IActionResult Edit(int id)
        {
            var role = _context.Roles.Include(r => r.RolesPermisos).FirstOrDefault(r => r.Idrol == id);
            if (role == null)
            {
                return NotFound();
            }

            // Obtener todos los permisos
            ViewBag.Permisos = _context.Permisos.ToList();

            // Permisos asignados al rol actual
            ViewBag.PermisosAsignados = role.RolesPermisos.Select(p => p.Idpermiso).ToList();

            // Enviar lista de nombres de roles existentes (excepto el rol actual)
            ViewBag.ExistingRoles = _context.Roles
                .Where(r => r.Idrol != id) // Excluir el rol actual
                .Select(r => r.Nombre)
                .ToList();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Role role, int[] selectedPermisos, bool isActive)
        {
            if (id != role.Idrol)
            {
                return NotFound();
            }

            // Validación: Verificar si el nombre ya existe en otro rol
            if (_context.Roles.Any(r => r.Nombre == role.Nombre && r.Idrol != id))
            {
                ModelState.AddModelError("Nombre", "Ya existe un rol con este nombre.");
            }

            // Validación: Verificar si se seleccionaron permisos
            if (selectedPermisos == null || selectedPermisos.Length == 0)
            {
                ModelState.AddModelError("Permisos", "Debe seleccionar al menos un permiso.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar propiedades del rol
                    role.IsActive = isActive;
                    _context.Update(role);
                    _context.SaveChanges();

                    // Actualizar permisos del rol
                    var existingPermissions = _context.RolesPermisos.Where(rp => rp.Idrol == id).ToList();
                    _context.RolesPermisos.RemoveRange(existingPermissions);

                    foreach (var permisoId in selectedPermisos)
                    {
                        var rolPermiso = new RolesPermiso
                        {
                            Idrol = role.Idrol,
                            Idpermiso = permisoId
                        };
                        _context.RolesPermisos.Add(rolPermiso);
                    }
                    _context.SaveChanges();

                    TempData["Success"] = "Rol actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Roles.Any(e => e.Idrol == role.Idrol))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, volver a cargar la vista con datos necesarios
            ViewBag.Permisos = _context.Permisos.ToList();
            ViewBag.PermisosAsignados = selectedPermisos;

            return View(role);
        }


        public IActionResult DeletePartial(int id)
        {
            var rol = _context.Roles
                              .Include(r => r.RolesPermisos)
                              .FirstOrDefault(r => r.Idrol == id);

            if (rol == null)
            {
                return NotFound();
            }

            var usuariosConRol = _context.Usuarios.Where(u => u.Idrol == id).ToList();
            ViewBag.HasUsers = usuariosConRol.Any();

            return PartialView("_DeletePartial", rol);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(m => m.Idrol == id);

            if (rol == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", rol);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine($"ID recibido: {id}"); // Agregar un log para depuración.

            var rol = await _context.Roles
                .Include(r => r.RolesPermisos)
                .Include(r => r.Usuarios)
                .FirstOrDefaultAsync(r => r.Idrol == id);

            if (rol == null)
            {
                return Json(new { success = false, message = "El rol no fue encontrado." });
            }

            if (rol.Usuarios.Any())
            {
                return Json(new { success = false, message = "No se puede eliminar el rol porque tiene usuarios asociados." });
            }

            try
            {
                if (rol.RolesPermisos.Any())
                {
                    _context.RolesPermisos.RemoveRange(rol.RolesPermisos);
                }

                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Rol eliminado con éxito." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar el rol: " + ex.Message });
            }
        }
    }
}