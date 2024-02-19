using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using System.Diagnostics;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        public LoginController(ILogger<HomeController> logger, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Accediendo al método Index del controlador Login.");
                return View(new LoginViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al acceder al método Index del controlador Login.");
                return BadRequest();
            }
        }


        [HttpPost]
        public IActionResult Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Mensaje"] = "Por favor, complete todos los campos.";
                _logger.LogWarning("ModelState no válido en el método Index del controlador Login.");
                return View("Index", loginViewModel);
            }
            try
            {
                var usuarioLogin = _usuarioRepository.ObtenerUsuarioPorCredenciales(loginViewModel.NombreDeUsuario!, loginViewModel.Contrasenia!);
                if (usuarioLogin == null)
                {
                    TempData["Mensaje"] = "Credenciales inválidas. Intente nuevamente.";
                    _logger.LogWarning("Intento de acceso inválido - Usuario: " + loginViewModel.NombreDeUsuario + " - Clave ingresada: " + loginViewModel.Contrasenia);
                    return View("Index", loginViewModel);
                }
                else
                {
                    LogearUsuario(usuarioLogin);
                    _logger.LogInformation("El usuario " + loginViewModel.NombreDeUsuario + " ingreso correctamente!");
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Ocurrió un error al procesar la solicitud. Por favor, inténtalo nuevamente más tarde.";
                _logger.LogError(ex, "Error al procesar la solicitud en el método Index del controlador Login.");
                return View("Index", loginViewModel);
            }
        }

        private void LogearUsuario(Usuario user)
        {
            try
            {
                HttpContext.Session.SetInt32("IdUsuario", user.IdUsuarioM);
                HttpContext.Session.SetString("Usuario", user.NombreDeUsuarioM!);
                HttpContext.Session.SetString("Rol", user.RolM.ToString());
                _logger.LogInformation($"El usuario {user.NombreDeUsuarioM} se ha registrado en la sesión con ID: {user.IdUsuarioM} y rol: {user.RolM}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión del usuario en la sesión");
                throw; // Es importante relanzar la excepción para que el controlador pueda manejarla adecuadamente
            }
        }


        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                TempData["Mensaje"] = "¡La sesión se cerró exitosamente! ¡Vuelve pronto!";
                _logger.LogInformation("La sesión se cerró exitosamente para el usuario.");
                return RedirectToAction("Index", "Login"); // Redirige a donde quieras tras cerrar sesión
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Ocurrió un error al cerrar sesión. Por favor, inténtalo nuevamente más tarde.";
                _logger.LogError(ex, "Error al cerrar sesión del usuario");
                return RedirectToAction("Index", "Login"); // Puedes redirigir a una página de error en caso de fallo
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
