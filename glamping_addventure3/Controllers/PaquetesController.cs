using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using glamping_addventure3.Models;
using Microsoft.AspNetCore.Authorization;

namespace glamping_addventure3.Controllers
{
    [Authorize]
    public class PaquetesController : Controller
    {
        private readonly GlampingAddventure3Context _context;

        public PaquetesController(GlampingAddventure3Context context)
        {
            _context = context;
        }
        // GET: Paquetes
        public async Task<IActionResult> Index()
        {
            var Paquetes = await _context.Paquetes
           .ToListAsync();
            foreach (var paquete in Paquetes)
            {
                if (paquete.ImagenPaquete != null)
                {
                    string imageBase64Data = Convert.ToBase64String(paquete.ImagenPaquete);
                    string ImagenDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                    paquete.ImagenDataURL = ImagenDataURL;
                }
            }

            var glampingAddventuresContext = _context.Paquetes.Include(p => p.IdhabitacionNavigation).Include(p => p.IdservicioNavigation);
            return View(await glampingAddventuresContext.ToListAsync());
        }

        // GET: Paquetes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes

                .Include(p => p.IdhabitacionNavigation)
                .Include(p => p.IdservicioNavigation)
                .FirstOrDefaultAsync(m => m.Idpaquete == id);
            if (paquete == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsPartial", paquete);
        }

        // POST: Paquetes/Create
        public IActionResult Create()
        {
            var habitacionesDisponibles = _context.Habitacions
                    .Where(h => !_context.Paquetes.Any(p => p.Idhabitacion == h.Idhabitacion) && h.Estado)
                    .ToList();

            // Verificar si no hay habitaciones disponibles
            if (habitacionesDisponibles == null || !habitacionesDisponibles.Any())
            {
                ViewData["HabitacionesMensaje"] = "No hay habitaciones disponibles";
                ViewBag.Habitaciones = new List<object>();
            }
            else
            {
                // Pasar las habitaciones filtradas al SelectList
                ViewData["Idhabitacion"] = new SelectList(habitacionesDisponibles, "Idhabitacion", "NombreHabitacion");
            }

            var serviciosDisponibles = _context.Servicios
                     .Where(s => !_context.Paquetes.Any(p => p.Idservicio == s.Idservicio) && s.Estado)
                     .ToList();

            if (!serviciosDisponibles.Any())
            {
                ViewData["ServiciosMensaje"] = "No hay servicios disponibles";
                ViewData["Idservicio"] = null;
            }
            else
            {
                ViewData["Idservicio"] = new SelectList(serviciosDisponibles, "Idservicio", "NombreServicio");
            }

            ViewData["Estados"] = new List<SelectListItem>{
                new SelectListItem { Value = "true", Text = "Activo" },
                new SelectListItem { Value = "false", Text = "Inactivo" }
            };
            return View();
        }

        // POST: Paquetes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idpaquete,NombrePaquete,ImagenPaquete,Descripcion,Idhabitacion,Idservicio,Precio,Estado")] Paquete paquete, IFormFile ImagenPaquete)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (ImagenPaquete != null && ImagenPaquete.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImagenPaquete.CopyToAsync(memoryStream);
                            paquete.ImagenPaquete = memoryStream.ToArray(); // Guarda la imagen como un arreglo de bytes
                        }
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrio un error al guardar algunos cambios" + ex.Message);
                }

                bool estadoSeleccionado = paquete.Estado;

                _context.Add(paquete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var habitacionesDisponibles = _context.Habitacions
                    .Where(h => !_context.Paquetes.Any(p => p.Idhabitacion == h.Idhabitacion) && h.Estado)
                    .ToList();

            // Verificar si no hay habitaciones disponibles
            if (!habitacionesDisponibles.Any())
            {
                ViewData["HabitacionesMensaje"] = "No hay habitaciones disponibles.";
                ViewData["HabitacionesMensaje2"] = "No se puede crear el paquete.";
                ViewData["Idhabitacion"] = null; // No enviar SelectList
            }
            else
            {
                // Pasar las habitaciones filtradas al SelectList
                ViewData["Idhabitacion"] = new SelectList(habitacionesDisponibles, "Idhabitacion", "NombreHabitacion");
            }

            var serviciosDisponibles = _context.Servicios
                     .Where(s => !_context.Paquetes.Any(p => p.Idservicio == s.Idservicio) && s.Estado)
                     .ToList();

            if (!serviciosDisponibles.Any())
            {
                ViewData["ServiciosMensaje"] = "No hay servicios disponibles";
                ViewData["ServiciosMensaje2"] = "No se puede crear el paquete.";
                ViewData["Idservicio"] = null;
            }
            else
            {
                ViewData["Idservicio"] = new SelectList(serviciosDisponibles, "Idservicio", "NombreServicio");
            }

            ViewData["Estados"] = new List<SelectListItem>{
                new SelectListItem { Value = "true", Text = "Activo" },
                new SelectListItem { Value = "false", Text = "Inactivo" }
            };
            return View();
        }

        // GET: Paquetes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete == null)
            {
                return NotFound();
            }


            ViewData["Idhabitacion"] = new SelectList(_context.Habitacions
                    .Where(h => h.Estado == true &&
                          (!_context.Paquetes.Any(p => p.Idhabitacion == h.Idhabitacion)
                                               || h.Idhabitacion == paquete.Idhabitacion)), "Idhabitacion", "NombreHabitacion", paquete.Idhabitacion);

            ViewData["Idservicio"] = new SelectList(_context.Servicios
                    .Where(s => s.Estado == true &&
                          (!_context.Paquetes.Any(p => p.Idservicio == s.Idservicio)
                                               || s.Idservicio == paquete.Idservicio)), "Idservicio", "NombreServicio", paquete.Idservicio);

            ViewData["Estados"] = new List<SelectListItem>{
                new SelectListItem { Value = "true", Text = "Activo" },
                new SelectListItem { Value = "false", Text = "Inactivo" }
            };
            return View(paquete);
        }

        // POST: Paquetes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idpaquete,NombrePaquete,ImagenPaquete,Descripcion,Idhabitacion,Idservicio,Precio,Estado")] Paquete paquete, IFormFile? ImagenPaquete)
        {
            if (id != paquete.Idpaquete)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (ImagenPaquete != null && ImagenPaquete.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await ImagenPaquete.CopyToAsync(memoryStream);
                        paquete.ImagenPaquete = memoryStream.ToArray();
                    }
                }
                else
                {
                    // Mantener la imagen existente si no se carga una nueva
                    var paqueteExistent = await _context.Paquetes.AsNoTracking()
                                             .FirstOrDefaultAsync(p => p.Idpaquete == id);
                    if (paqueteExistent != null)
                    {
                        paquete.ImagenPaquete = paqueteExistent.ImagenPaquete;
                    }
                }

                try
                {
                    _context.Update(paquete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaqueteExists(paquete.Idpaquete))
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
            ViewData["Idhabitacion"] = new SelectList(_context.Habitacions, "Idhabitacion", "NombreHabitacion", paquete.Idhabitacion);
            ViewData["Idservicio"] = new SelectList(_context.Servicios, "Idservicio", "NombreServicio", paquete.Idservicio);
            ViewData["Estados"] = new List<SelectListItem>{
                new SelectListItem { Value = "true", Text = "Activo" },
                new SelectListItem { Value = "false", Text = "Inactivo" }
            };
            return View(paquete);
        }

        // GET: Paquetes/Delete/5
        public IActionResult DeletePartial(int id)
        {
            var paquete = _context.Paquetes
                .FirstOrDefault(p => p.Idpaquete == id);

            if (paquete == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", paquete);  // Devolver la vista parcial
        }

        [HttpPost]
        public JsonResult DeleteConfirmed(int id)
        {
            var paquete = _context.Paquetes
                .Include(p => p.DetalleReservaPaquetes)  // Asegúrate de que esta relación esté correctamente configurada
                .FirstOrDefault(p => p.Idpaquete == id);

            if (paquete != null && paquete.DetalleReservaPaquetes.Any())
            {
                // Si el paquete está asociado a una reserva, devolver un mensaje de error
                return Json(new { success = false, message = "No se puede eliminar el paquete, está asociado a una reserva." });
            }

            // Si no está asociado a ninguna reserva, proceder con la eliminación
            _context.Paquetes.Remove(paquete);
            _context.SaveChanges();

            // Devolver un mensaje de éxito
            return Json(new { success = true, message = "Paquete eliminado con éxito." });
        }

        private bool PaqueteExists(int id)
        {
            return _context.Paquetes.Any(e => e.Idpaquete == id);
        }

        public IActionResult CambiarEstado(int id)
        {
            Paquete paquete = _context.Paquetes.Find(id);

            if (paquete == null)
            {
                return RedirectToAction("Index");
            }

            paquete.Estado = !paquete.Estado;
            _context.Update(paquete);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetCostos(int habitacionId, int servicioId)
        {
            // Simulación de obtener costos desde la base de datos
            var habitacionCosto = _context.Habitacions
                .Where(h => h.Idhabitacion == habitacionId)
                .Select(h => h.Costo)
                .FirstOrDefault();

            var servicioCosto = _context.Servicios
                .Where(s => s.Idservicio == servicioId)
                .Select(s => s.Costo)
                .FirstOrDefault();

            return Json(new { habitacionCosto, servicioCosto });
        }

    }
}
