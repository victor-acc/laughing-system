using Microsoft.AspNetCore.Mvc;
using Glamping_Addventure.Models.Recursos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;
using Glamping_Addventure3.Models.Servicios.Contrato;
using glamping_addventure3.Models;

namespace glamping_addventure3.Controllers
{

    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;

        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        public async Task<IActionResult> Registrarse()
        {
            // Obtiene los roles activos para pasarlos a la vista
            var rolesActivos = await _usuarioServicio.GetRolesActivos();
            ViewBag.Roles = rolesActivos;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo, int Idrol, string confirmarContrasena)
        {
            // Validar si el modelo es nulo o tiene errores
            if (modelo == null)
            {
                ModelState.AddModelError("", "El formulario no es válido.");
                return View(modelo);
            }

            // Validar si se seleccionó un rol
            if (Idrol <= 0)
            {
                ModelState.AddModelError("Idrol", "Debe seleccionar un rol.");
            }
            else
            {
                // Verificar si el rol está activo
                var rol = await _usuarioServicio.GetRolPorId(Idrol);
                if (rol == null)
                {
                    ModelState.AddModelError("Idrol", "El rol seleccionado no existe.");
                }
                else if (!rol.IsActive)
                {
                    ModelState.AddModelError("Idrol", "El rol seleccionado está inactivo.");
                }
            }

            // Validación de contraseñas
            if (string.IsNullOrWhiteSpace(modelo.Contrasena))
            {
                ModelState.AddModelError("Contrasena", "La contraseña no puede estar vacía.");
            }
            else if (modelo.Contrasena != confirmarContrasena)
            {
                ModelState.AddModelError("Contrasena", "Las contraseñas no coinciden.");
            }

            // Validar si el correo ya está registrado
            if (!string.IsNullOrWhiteSpace(modelo.Email))
            {
                var usuarioExistente = await _usuarioServicio.GetUsuarioPorEmail(modelo.Email);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Debe proporcionar un correo electrónico válido.");
            }

            // Validar si el número de documento ya está registrado
            if (!string.IsNullOrWhiteSpace(modelo.NumeroDocumento?.ToString()))
            {
                var documentoExistente = await _usuarioServicio.GetUsuarioPorDocumento(modelo.NumeroDocumento);
                if (documentoExistente != null)
                {
                    ModelState.AddModelError("NumeroDocumento", "El número de documento ya está registrado.");
                }
            }

            // Si hay errores de validación, retornar la vista con el modelo
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            // Encriptar la contraseña antes de guardarla
            modelo.Contrasena = Utilidades.EncriptarClave(modelo.Contrasena);
            modelo.Idrol = Idrol;

            // Guardar el usuario en la base de datos
            Usuario usuarioCreado = await _usuarioServicio.SaveUsuario(modelo);
            if (usuarioCreado != null && usuarioCreado.Idusuario > 0)
            {
                TempData["MensajeExito"] = "Usuario creado exitosamente.";
                return RedirectToAction("IniciarSesion", "Inicio");
            }

            // Si no se pudo crear el usuario, retornar un mensaje de error
            ModelState.AddModelError("", "Ocurrió un error al intentar registrar el usuario.");
            return View(modelo);
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string email, string contrasena)
        {
            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(email, Utilidades.EncriptarClave(contrasena));

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            // Verifica si IdrolNavigation es nulo
            string rolNombre = usuario_encontrado.IdrolNavigation?.Nombre ?? "Usuario"; // Asigna un valor por defecto si es null

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario),
                new Claim(ClaimTypes.Role, rolNombre) // Usa el nombre del rol o un valor por defecto
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // Guarda la última actividad del usuario en la sesión
            HttpContext.Session.SetString("LastActivity", DateTime.UtcNow.ToString());

            return RedirectToAction("Index", "Home");
        }

        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }

        [HttpGet]
        public IActionResult ObtenerCiudades(string pais)
        {
            var ciudades = new Dictionary<string, List<string>>
    {
        { "Colombia", new List<string> { "Bogotá", "Medellín", "Cali" } },
        { "Estados Unidos", new List<string> { "Nueva York", "Los Ángeles", "Chicago" } },
        { "España", new List<string> { "Madrid", "Barcelona", "Valencia" } },
        { "México", new List<string> { "Ciudad de México", "Guadalajara", "Monterrey" } },
    };

            if (ciudades.ContainsKey(pais))
            {
                return Json(ciudades[pais]);
            }

            return Json(new List<string>()); // Retorna una lista vacía si no hay ciudades para el país
        }


    }
}