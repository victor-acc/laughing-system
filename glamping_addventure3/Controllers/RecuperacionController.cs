using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using glamping_addventure3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Glamping_Addventure3.Services;
using Glamping_Addventure.Models.Recursos;
using Microsoft.AspNetCore.Authorization;

namespace glamping_addventure3.Controllers
{
    [Authorize]
    public class RecuperacionController : Controller
    {
        private readonly GlampingAddventure3Context _context;
        private readonly IEmailService _emailService;  // Inyectamos el servicio de correo electrónico

        public RecuperacionController(GlampingAddventure3Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // Vista para solicitar el código de recuperación
        public IActionResult SolicitarRecuperacion()
        {
            return View();
        }

        // Acción para enviar el código de recuperación
        [HttpPost]
        public async Task<IActionResult> SolicitarRecuperacion(string email)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = "El correo electrónico no está registrado.";
                return View();
            }

            // Genera un código aleatorio
            var codigo = Guid.NewGuid().ToString().Substring(0, 6); // Código de 6 caracteres

            // Guarda el código de recuperación en la base de datos
            var codigoRecuperacion = new CodigoRecuperacion
            {
                UsuarioId = usuario.Idusuario,
                Codigo = codigo,
                FechaExpiracion = DateTime.Now.AddMinutes(30), // Código válido por 30 minutos
                Usado = false
            };

            _context.CodigosRecuperacion.Add(codigoRecuperacion);
            await _context.SaveChangesAsync();

            // Enviar el correo con el código de recuperación
            var subject = "Código de Recuperación de Contraseña";
            var body = $"Tu código de recuperación es: <strong>{codigo}</strong>. Este código es válido por 30 minutos.";

            try
            {
                await _emailService.EnviarCorreoRecuperacion(email, subject, body);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al enviar el correo: {ex.Message}";
                return View();
            }

            // Redirigir a la vista donde se ingresa el código de recuperación
            return RedirectToAction("IngresarCodigo", new { email = usuario.Email });
        }

        // Acción para verificar el código ingresado
        public IActionResult IngresarCodigo(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IngresarCodigo(string email, string codigoIngresado)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = "El usuario no existe.";
                return View();
            }

            var codigoRecuperacion = await _context.CodigosRecuperacion
                .FirstOrDefaultAsync(c => c.UsuarioId == usuario.Idusuario && c.Codigo == codigoIngresado && c.FechaExpiracion > DateTime.Now && !c.Usado);

            if (codigoRecuperacion == null)
            {
                ViewBag.ErrorMessage = "El código es inválido o ha expirado.";
                return View();
            }

            // Marcar el código como usado
            codigoRecuperacion.Usado = true;
            _context.CodigosRecuperacion.Update(codigoRecuperacion);
            await _context.SaveChangesAsync();

            // Redirigir a la vista para cambiar la contraseña
            return RedirectToAction("CambiarContrasena", new { email = usuario.Email });
        }

        public IActionResult CambiarContrasena(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(string email, string nuevaContrasena)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                ViewBag.ErrorMessage = "El usuario no existe.";
                return View();
            }

            // Encriptar la nueva contraseña antes de guardarla
            usuario.Contrasena = Utilidades.EncriptarClave(nuevaContrasena);
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("IniciarSesion", "Inicio");
        }
    }
}