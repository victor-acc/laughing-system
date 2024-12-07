using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Glamping_Addventure.Models.Servicios.Implementación;
using Microsoft.AspNetCore.Authentication;
using Glamping_Addventure3.Models.Servicios.Contrato;
using glamping_addventure3.Models;
using Glamping_Addventure3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura el contexto de la base de datos
builder.Services.AddDbContext<GlampingAddventure3Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggingDatabase")));

// Registra servicios personalizados
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEmailService, EmailService>(); // Asegúrate de registrar el servicio de correo
builder.Services.AddScoped<IUsuarioService, UsuarioService>();




// Configuración de autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion";  // Redirige a esta página si no está autenticado
        options.AccessDeniedPath = "/Home/Index"; // Redirige a una página de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true; // Permite renovar la sesión si el usuario está activo
    });

// Configuración de autorización para roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administradores", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("Recepcionistas", policy => policy.RequireRole("Recepcionista"));
});

// Configuración de la sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Cambia esto según tus necesidades
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Necesario para que la sesión funcione
});

// Configuración de filtros para controlar el almacenamiento en caché
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None,
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5187")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Antes de app.UseAuthorization();
app.UseCors("PermitirLocalhost");
var defaultCulture = new CultureInfo("es-MX");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};

app.UseRequestLocalization(localizationOptions);


// Añadir el middleware para gestionar el tiempo de sesión
app.UseSession(); // Asegúrate de usar la sesión antes de usar el middleware
app.UseMiddleware<SessionTimeoutMiddleware>(); // Añadir el middleware de sesión

// Configura el middleware para asegurar que los usuarios no autenticados sean redirigidos
app.UseAuthentication();
app.UseAuthorization();

// Configuración de rutas para la aplicación
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

// Ruta para el controlador de recuperación de contraseña
app.MapControllerRoute(
    name: "recuperacion",
    pattern: "Recuperacion/{action=SolicitarRecuperacion}/{id?}",
    defaults: new { controller = "Recuperacion", action = "SolicitarRecuperacion" });

app.Run();

// Middleware para gestionar el tiempo de sesión
public class SessionTimeoutMiddleware
{
    private readonly RequestDelegate _next;

    public SessionTimeoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var lastActivityString = context.Session.GetString("LastActivity");
            if (lastActivityString != null)
            {
                var lastActivity = DateTime.Parse(lastActivityString);
                if (DateTime.UtcNow - lastActivity > TimeSpan.FromMinutes(20)) // Cambia el tiempo aquí según tu necesidad
                {
                    await Logout(context); // Cerrar sesión si la sesión ha expirado
                }
            }

            // Actualiza la última actividad
            context.Session.SetString("LastActivity", DateTime.UtcNow.ToString());
        }

        await _next(context);
    }

    private async Task Logout(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Cerrar sesión
        context.Response.Redirect("/Inicio/IniciarSesion"); // Redirigir a la página de inicio de sesión
    }
}
