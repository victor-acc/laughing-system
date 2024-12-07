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
    public class HabitacionsController : Controller
    {
        private readonly GlampingAddventure3Context _context;

        public HabitacionsController(GlampingAddventure3Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> ValidarDisponibilidad()
        {
            bool existenMuebles = await _context.Muebles.AnyAsync();
            bool existenTiposdeHabitacions = await _context.TipodeHabitacions.AnyAsync();

            return Json(new
            {
                existenMuebles,
                existenTiposdeHabitacions
            });
        }
        // GET: Habitacions
        public async Task<IActionResult> Index()
        {

            var habitaciones = await _context.Habitacions
            .Include(h => h.IdtipodeHabitacionNavigation)
            .ToListAsync();
            foreach (var habitacion in habitaciones)
            {
                if (habitacion.ImagenHabitacion != null)
                {
                    string imageBase64Data = Convert.ToBase64String(habitacion.ImagenHabitacion);
                    string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                    habitacion.ImagenDataURL = imageDataURL;
                }
            }

            return View(habitaciones);
        }
        // GET: Habitacions/Details/5
        // GET: Habitacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitacions
                .Include(h => h.IdtipodeHabitacionNavigation)
                .Include(h => h.HabitacionMuebles)
                    .ThenInclude(hm => hm.IdmuebleNavigation)
                .FirstOrDefaultAsync(m => m.Idhabitacion == id);

            if (habitacion == null)
            {
                return NotFound();
            }

            if (habitacion.ImagenHabitacion != null && habitacion.ImagenHabitacion.Length > 0)
            {
                string imageBase64Data = Convert.ToBase64String(habitacion.ImagenHabitacion);
                string imageDataURL = $"data:image/png;base64,{imageBase64Data}";
                ViewData["ImageDataURL"] = imageDataURL;
            }
            else
            {
                ViewData["ImageDataURL"] = "/path/to/default-image.png"; // Imagen predeterminada si no hay datos
            }

            // Agrupación de muebles considerando la cantidad usada
            var mueblesAgrupados = habitacion.HabitacionMuebles
    .Where(hm => hm.IdmuebleNavigation != null) // Filtrar muebles válidos
    .GroupBy(hm => hm.IdmuebleNavigation.NombreMueble)
    .Select(g => new
    {
        NombreMueble = g.Key,
        Cantidad = g.Sum(hm => hm.CantidadUsada) // Sumar correctamente la cantidad usada
    })
    .ToList();

            ViewData["MueblesAgrupados"] = mueblesAgrupados;

            return PartialView("_DetailsPartial", habitacion);
        }



        // GET: Habitacions/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IdtipodeHabitacion"] = new SelectList(_context.TipodeHabitacions.Where(s => s.Estado == true), "IdtipodeHabitacion", "NombreTipodeHabitacion");
            ViewData["MueblesDisponibles"] = _context.Muebles.Where(m => m.CantidadDisponible > 0).ToList();
            return View();
        }
        private void CargarViewData(Habitacion? habitacion = null)
        {
            ViewData["IdtipodeHabitacion"] = new SelectList(_context.TipodeHabitacions.Where(s => s.Estado == true), "IdtipodeHabitacion", "NombreTipodeHabitacion", habitacion?.IdtipodeHabitacion);
            ViewData["MueblesDisponibles"] = _context.Muebles.ToList();
        }
        // POST: Habitacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idhabitacion,NombreHabitacion,ImagenHabitacion,Descripcion,Costo,IdtipodeHabitacion,Estado")] Habitacion habitacion, IFormFile ImageFile, Dictionary<int, int> SelectedMuebles)
        {
            if (habitacion.Costo < 0)
            {
                ModelState.AddModelError("Costo", "El costo no puede ser negativo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImageFile.CopyToAsync(memoryStream);
                            habitacion.ImagenHabitacion = memoryStream.ToArray();
                        }
                    }

                    _context.Add(habitacion);
                    await _context.SaveChangesAsync();

                    // Guardar relación con muebles y actualizar la cantidad disponible
                    foreach (var muebleEntry in SelectedMuebles)
                    {
                        int muebleId = muebleEntry.Key;
                        int cantidadSeleccionada = muebleEntry.Value;

                        var mueble = await _context.Muebles.FindAsync(muebleId);
                        if (mueble != null && mueble.CantidadDisponible >= cantidadSeleccionada)
                        {
                            // Crear una sola entrada con la cantidad usada
                            var habitacionMueble = new HabitacionMueble
                            {
                                Idhabitacion = habitacion.Idhabitacion,
                                Idmueble = muebleId,
                                CantidadUsada = cantidadSeleccionada // Aquí guardamos la cantidad
                            };

                            _context.HabitacionMuebles.Add(habitacionMueble);

                            // Actualizar la cantidad disponible del mueble
                            mueble.CantidadDisponible -= cantidadSeleccionada;
                            _context.Muebles.Update(mueble);
                        }
                        else
                        {
                            ModelState.AddModelError("", $"La cantidad disponible de {mueble?.NombreMueble ?? "mueble desconocido"} es insuficiente.");
                            CargarViewData(habitacion);
                            return View(habitacion);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar los cambios. Detalles: " + ex.Message);
                }
            }

            CargarViewData();
            return View(habitacion);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitacions
                .Include(h => h.HabitacionMuebles)
                .ThenInclude(hm => hm.IdmuebleNavigation)
                .FirstOrDefaultAsync(h => h.Idhabitacion == id);

            if (habitacion == null)
            {
                return NotFound();
            }

            ViewData["IdtipodeHabitacion"] = new SelectList(_context.TipodeHabitacions, "IdtipodeHabitacion", "NombreTipodeHabitacion");
            ViewData["MueblesDisponibles"] = _context.Muebles
    .AsEnumerable()
    .Where(m => m.CantidadDisponible > 0 || habitacion.HabitacionMuebles.Any(hm => hm.Idmueble == m.Idmueble))
    .ToList();
            return View(habitacion);
        }

        // POST: Habitacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idhabitacion,NombreHabitacion,ImagenHabitacion,Descripcion,Costo,IdtipodeHabitacion,Estado")] Habitacion habitacion, IFormFile ImageFile, List<int> SelectedMuebles)
        {
            if (id != habitacion.Idhabitacion)
            {
                return NotFound();
            }

            if (habitacion.Costo < 0)
            {
                ModelState.AddModelError("Costo", "El costo no puede ser negativo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImageFile.CopyToAsync(memoryStream);
                            habitacion.ImagenHabitacion = memoryStream.ToArray();
                        }
                    }

                    _context.Update(habitacion);

                    // Actualizar muebles asociados
                    var mueblesActuales = _context.HabitacionMuebles.Where(hm => hm.Idhabitacion == id).ToList();

                    // Eliminar muebles deseleccionados
                    foreach (var muebleActual in mueblesActuales)
                    {
                        if (muebleActual.Idmueble.HasValue && !SelectedMuebles.Contains(muebleActual.Idmueble.Value))
                        {
                            var mueble = await _context.Muebles.FindAsync(muebleActual.Idmueble);
                            if (mueble != null) // Verificación para evitar desreferencia nula
                            {
                                mueble.CantidadDisponible++;
                                _context.HabitacionMuebles.Remove(muebleActual);
                            }
                        }
                    }

                    // Agregar nuevos muebles seleccionados
                    foreach (var muebleId in SelectedMuebles)
                    {
                        if (!mueblesActuales.Any(m => m.Idmueble == muebleId))
                        {
                            var mueble = await _context.Muebles.FindAsync(muebleId);
                            if (mueble != null && mueble.CantidadDisponible > 0)
                            {
                                var habitacionMueble = new HabitacionMueble
                                {
                                    Idhabitacion = habitacion.Idhabitacion,
                                    Idmueble = muebleId
                                };
                                _context.HabitacionMuebles.Add(habitacionMueble);
                                mueble.CantidadDisponible--;
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacionExists(habitacion.Idhabitacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar los cambios. Detalles: " + ex.Message);
                }
            }

            ViewData["IdtipodeHabitacion"] = new SelectList(_context.TipodeHabitacions, "IdtipodeHabitacion", "IdtipodeHabitacion", habitacion.IdtipodeHabitacion);
            ViewData["MueblesDisponibles"] = _context.Muebles.Where(m => m.CantidadDisponible > 0 || habitacion.HabitacionMuebles.Any(hm => hm.Idmueble == m.Idmueble)).ToList();
            return View(habitacion);
        }
        private bool HabitacionExists(int id)
        {
            return _context.Habitacions.Any(e => e.Idhabitacion == id);
        }
        // GET: Habitacions/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitacions
                .Include(h => h.IdtipodeHabitacionNavigation)
                .FirstOrDefaultAsync(m => m.Idhabitacion == id);

            if (habitacion == null)
            {
                return NotFound();
            }

            if (habitacion.ImagenHabitacion != null)
            {
                ViewData["ImageDataURL"] = "data:image/png;base64," + Convert.ToBase64String(habitacion.ImagenHabitacion);
            }

            return PartialView("_DeletePartial", habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitacion = await _context.Habitacions
                .Include(h => h.HabitacionMuebles)
                .FirstOrDefaultAsync(h => h.Idhabitacion == id);

            if (habitacion == null)
            {
                return Json(new { success = false, message = "La habitación no fue encontrada." });
            }

            try
            {
                // Restaurar la cantidad de muebles.
                foreach (var habitacionMueble in habitacion.HabitacionMuebles)
                {
                    var mueble = await _context.Muebles.FindAsync(habitacionMueble.Idmueble);
                    if (mueble != null)
                    {
                        mueble.CantidadDisponible++;
                    }
                    _context.HabitacionMuebles.Remove(habitacionMueble);
                }

                _context.Habitacions.Remove(habitacion);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var habitacion = _context.Habitacions.Find(id);
            if (habitacion != null)
            {
                habitacion.Estado = !habitacion.Estado;
                _context.Update(habitacion);
                _context.SaveChanges();
                return Json(new { success = true, estado = habitacion.Estado });
            }
            return Json(new { success = false });
        }
    }
}
