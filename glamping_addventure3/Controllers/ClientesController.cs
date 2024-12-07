using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using glamping_addventure3.Models;
using Microsoft.AspNetCore.Authorization;

namespace reserva3.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly GlampingAddventure3Context _context;

        public ClientesController(GlampingAddventure3Context context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            
            var glamping3Context = _context.Clientes.Include(c => c.IdrolNavigation);
            return View(await glamping3Context.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["Idrol"] = new SelectList(_context.Roles, "Idrol", "Idrol");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroDocumento,TipoDocumento,Nombre,Apellido,Direccion,Email,Telefono,Estado,Idrol")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();

                // Después de guardar el cliente, redirigir a la vista de creación de reserva
                return RedirectToAction("Create", "Reservas");
            }

            ViewData["Idrol"] = new SelectList(_context.Roles, "Idrol", "Idrol", cliente.Idrol);
            return View(cliente);
        }


        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("NroDocumento,,Nombre,Apellido,Direccion,Email,Telefono,Estado,Idrol")] Cliente cliente)
        {
            if (id != cliente.NroDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.NroDocumento))
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
            ViewData["Idrol"] = new SelectList(_context.Roles, "Idrol", "Idrol", cliente.Idrol);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(string id)
        {
            return _context.Clientes.Any(e => e.NroDocumento == id);
        }
    }
}
