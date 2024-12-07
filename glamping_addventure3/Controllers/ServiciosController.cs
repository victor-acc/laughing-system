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
    public class ServiciosController : Controller
    {
        private readonly GlampingAddventure3Context _context;

        public ServiciosController(GlampingAddventure3Context context)
        {
            _context = context;
        }
        // GET: Servicios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Servicios.ToListAsync());
        }

        // GET: Servicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(m => m.Idservicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsPartial", servicio);
        }

        // GET: Servicios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idservicio,NombreServicio,Descripcion,Duracion,CantidadMaximaPersonas,Costo,Estado")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(servicio);
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idservicio,NombreServicio,Descripcion,Duracion,CantidadMaximaPersonas,Costo,Estado")] Servicio servicio)
        {
            if (id != servicio.Idservicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioExists(servicio.Idservicio))
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
            return View(servicio);
        }
        [HttpGet]
        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(m => m.Idservicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine($"ID recibido: {id}"); // Agregar un log para depuración.

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(h => h.Idservicio == id);

            if (servicio == null)
            {
                return Json(new { success = false, message = "El servicio no fue encontrado." });
            }

            try
            {
                _context.Servicios.Remove(servicio);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

        private bool ServicioExists(int id)
        {
            return _context.Servicios.Any(e => e.Idservicio == id);
        }
        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var servicio = _context.Servicios.Find(id);
            if (servicio != null)
            {
                servicio.Estado = !servicio.Estado;
                _context.Update(servicio);
                _context.SaveChanges();
                return Json(new { success = true, estado = servicio.Estado });
            }
            return Json(new { success = false });
        }
    }
}
