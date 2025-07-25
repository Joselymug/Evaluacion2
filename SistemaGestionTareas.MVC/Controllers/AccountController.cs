using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SistemaGestionTareas.Consumer;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SistemaGestionTareas.MVC.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
            Crud<Usuario>.EndPoint = "https://localhost:7056/api/Usuarios";
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            var usuarios = Crud<Usuario>.GetAll();
            var usuario = usuarios.FirstOrDefault(u => u.CorreoElectronico == username && u.Contraseña == password);
            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                    new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")
                };
                // Aquí puedes establecer la sesión o el token de autenticación
                // Por ejemplo, usando HttpContext.Session o JWT
                return Redirect(returnUrl ?? "/Home/Index");
            }
            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            var usuarios = Crud<Usuario>.GetAll();
            if (usuarios.Any(u => u.CorreoElectronico == usuario.CorreoElectronico))
            {
                ModelState.AddModelError("CorreoElectronico", "El correo electrónico ya está en uso.");
                return View(usuario);
            }
            if (usuarios.Any(u => u.Nombre == usuario.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre de usuario ya está en uso.");
                return View(usuario);
            }


            if (ModelState.IsValid)
            {
                var nuevoUsuario = new Usuario
                {
                    Nombre = usuario.Nombre,
                    CorreoElectronico = usuario.CorreoElectronico,
                    Contraseña = usuario.Contraseña, // Asegúrate de hashear la contraseña antes de guardarla
                    Rol = "Cliente" // O el rol que desees asignar por defecto
                };

                var usuarioCreado = Crud<Usuario>.Create(nuevoUsuario);

                if (usuarioCreado != null && usuarioCreado?.Id > 0)
                {
                    TempData["SuccessMessage"] = "Registro exitoso. Ahora puedes iniciar sesión.";
                    return RedirectToAction("Login");
                }
                ViewData["ErrorMessage"] = "Error al crear el usuario. Por favor, inténtalo de nuevo.";
                return View();
            }
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Si estás usando autenticación basada en cookies
            return RedirectToAction("Index", "Home");
        }
    }
}