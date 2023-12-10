using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class LoginController : Controller
    {
        private readonly IUsuarioRepository usuarioRepository;
        public LoginController()
        {
            usuarioRepository = new UsuarioRepository();
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Por favor, complete todos los campos.");
                return View("Index", loginViewModel);
            }

            var usuarioLogin = usuarioRepository.ObtenerUsuarioPorCredenciales(loginViewModel.NombreDeUsuario!, loginViewModel.Contrasenia!);
            // si el usuario no existe devuelvo al index
            if (usuarioLogin == null)
            {
                TempData["Mensaje"] = "Credenciales inválidas. Intente nuevamente.";
                return View("Index", loginViewModel);
            }
            else
            {
                //Registro el usuario
                LogearUsuario(usuarioLogin);
                return RedirectToAction("Index", "Home");
            }
        }

        private void LogearUsuario(Usuario user)
        {
            HttpContext.Session.SetInt32("IdUsuario", user.IdUsuario);
            HttpContext.Session.SetString("Usuario", user.NombreDeUsuario!);
            HttpContext.Session.SetString("Rol", user.Rol.ToString());
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Mensaje"] = "¡La sesión se cerró exitosamente! ¡Vuelve pronto!";
            return RedirectToAction("Index", "Login"); // Redirige a donde quieras tras cerrar sesión
        }
    }

}
