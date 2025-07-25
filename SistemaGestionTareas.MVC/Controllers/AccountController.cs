using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using SistemaGestionTareas.Consumer;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SistemaGestionTareas.MVC.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
            Crud<Usuario>.EndPoint = "https://localhost:7118/api/Usuarios";
        }

        // Mostrar la vista de Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Procesar el login del usuario
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            var usuarios = Crud<Usuario>.GetAll();
            var usuario = usuarios.FirstOrDefault(u => u.CorreoElectronico == username);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.Contraseña))
            {
                // Crear las reclamaciones (claims)
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
            new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")
        };

                // Crear los objetos de autenticación
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Mantener la sesión
                };

                // Iniciar sesión
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirigir al usuario a la página de retorno o a la página de inicio
                return Redirect(returnUrl ?? "/Home/Index");
            }

            // Si las credenciales son incorrectas
            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View();
        }


        // Mostrar la vista de Registro
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Procesar el registro del usuario
        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            var usuarios = Crud<Usuario>.GetAll();

            // Validar si el correo ya está registrado
            if (usuarios.Any(u => u.CorreoElectronico == usuario.CorreoElectronico))
            {
                ModelState.AddModelError("CorreoElectronico", "El correo electrónico ya está en uso.");
                return View(usuario);
            }

            // Validar si el nombre de usuario ya está registrado
            if (usuarios.Any(u => u.Nombre == usuario.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre de usuario ya está en uso.");
                return View(usuario);
            }

            if (ModelState.IsValid)
            {
                // Hashear la contraseña del usuario antes de guardarla
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

                var nuevoUsuario = new Usuario
                {
                    Nombre = usuario.Nombre,
                    CorreoElectronico = usuario.CorreoElectronico,
                    Contraseña = hashedPassword, // Almacenar la contraseña hasheada
                    Rol = "Cliente" // O el rol que quieras asignar por defecto
                };

                // Crear el usuario
                var usuarioCreado = Crud<Usuario>.Create(nuevoUsuario);

                if (usuarioCreado != null && usuarioCreado?.Id > 0)
                {
                    TempData["SuccessMessage"] = "Registro exitoso. Ahora puedes iniciar sesión.";
                    return RedirectToAction("Login");
                }

                // Error al crear el usuario
                ViewData["ErrorMessage"] = "Error al crear el usuario. Por favor, inténtalo de nuevo.";
                return View();
            }
            return View(usuario);
        }

        // Cerrar sesión
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Cerrar sesión
            return RedirectToAction("Index", "Home"); // Redirigir a la página de inicio
        }
    }
}
