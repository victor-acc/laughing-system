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
    public class TipodeHabitacionsController : Controller
    {
        private readonly GlampingAddventure3Context _context;

        public TipodeHabitacionsController(GlampingAddventure3Context context)
        {
            _context = context;
        }
        // GET: TipodeHabitacions
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipodeHabitacions.ToListAsync());
        }


        // GET: TipodeHabitacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipodeHabitacion = await _context.TipodeHabitacions
                .FirstOrDefaultAsync(m => m.IdtipodeHabitacion == id);
            if (tipodeHabitacion == null)
            {
                return NotFound();
            }

            return PartialView("_DetailsPartial", tipodeHabitacion);
        }

        // GET: TipodeHabitacions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipodeHabitacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdtipodeHabitacion,NombreTipodeHabitacion,Descripcion,Estado")] TipodeHabitacion tipodeHabitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipodeHabitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipodeHabitacion);
        }

        // GET: TipodeHabitacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipodeHabitacion = await _context.TipodeHabitacions.FindAsync(id);
            if (tipodeHabitacion == null)
            {
                return NotFound();
            }
            return View(tipodeHabitacion);
        }

        // POST: TipodeHabitacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdtipodeHabitacion,NombreTipodeHabitacion,Descripcion,Estado")] TipodeHabitacion tipodeHabitacion)
        {
            if (id != tipodeHabitacion.IdtipodeHabitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipodeHabitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipodeHabitacionExists(tipodeHabitacion.IdtipodeHabitacion))
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
            return View(tipodeHabitacion);
        }
        [HttpGet]
        // GET: TipodeHabitacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipohabitacion = await _context.TipodeHabitacions
                .Include(t => t.Habitacions)
                .FirstOrDefaultAsync(m => m.IdtipodeHabitacion == id);
            if (tipohabitacion == null)
            {
                return NotFound();
            }

            return PartialView("_DeletePartial", tipohabitacion);
        }

        // POST: TipodeHabitacions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine($"ID recibido: {id}"); // Agregar un log para depuración.

            var tipohabitacion = await _context.TipodeHabitacions
                .FirstOrDefaultAsync(h => h.IdtipodeHabitacion == id);

            if (tipohabitacion == null)
            {
                return Json(new { success = false, message = "El tipo de habitacion no fue encontrado." });
            }

            try
            {
                _context.TipodeHabitacions.Remove(tipohabitacion);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

        private bool TipodeHabitacionExists(int id)
        {
            return _context.TipodeHabitacions.Any(e => e.IdtipodeHabitacion == id);
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var tipodehabitacion = _context.TipodeHabitacions.Find(id);
            if (tipodehabitacion != null)
            {
                tipodehabitacion.Estado = !tipodehabitacion.Estado;
                _context.Update(tipodehabitacion);
                _context.SaveChanges();
                return Json(new { success = true, estado = tipodehabitacion.Estado });
            }
            return Json(new { success = false });
        }
    }
}
