using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;


namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    return View();
                }
                else // verificación de otro rol necesario
                {
                    // int idUsuario = (int)HttpContext.Session.GetInt32("IdUsuario"); // 0 es un valor por defecto si no se encuentra el ID de usuario
                    // return RedirectToAction("ListarTablerosDeUsuarioEspecifico", "Tablero", new { idUsuario = idUsuario });
                    return RedirectToAction("Index", "Tablero");
                }
            }
            else
            {
                // si intenta ingresar forzadamente regresa al home
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}