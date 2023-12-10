using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class TableroController : Controller
    {
        private readonly TableroRepository tableroRepository;
        public TableroController()
        {
            tableroRepository = new TableroRepository();
        }

        public IActionResult Index()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    return View();
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    return View();
                }
                return RedirectToAction("AccesoDenegado", "Usuario");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public IActionResult MostrarTodosTablero()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var tablero = tableroRepository.ListarTodosTableros();
                    return View(tablero);
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult ListarTablerosDeUsuarioEspecifico(int idUsuario)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                var tablero = tableroRepository.ListarTablerosDeUsuarioEspecifico(idUsuario);
                return View(tablero);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public IActionResult AgregarTablero()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var viewModel = new CrearTableroViewModel(new Tablero());
                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult ConfirmarAgregarTablero(CrearTableroViewModel tableroViewModel)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        var tablero = new Tablero
                        {
                            NombreDeTablero = tableroViewModel.NombreDeTablero,
                            DescripcionDeTablero = tableroViewModel.DescripcionDeTablero
                        };
                        tableroRepository.CrearTablero(tablero);
                        return RedirectToAction("MostrarTodosTablero");
                    }
                    return View(tableroViewModel);
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult EliminarTablero(int id)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var tablero = tableroRepository.TreaerTableroPorId(id);
                    if (tablero == null)
                    {
                        return NotFound();
                    }
                    return View(tablero);
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult ConfirmarEliminar(Tablero tablero)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    tableroRepository.EliminarTableroPorId(tablero.IdTablero);
                    return RedirectToAction("MostrarTodosTablero");
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public IActionResult ModificarTablero(int id)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var tablero = tableroRepository.TreaerTableroPorId(id);
                    if (tablero == null)
                    {
                        return NotFound();
                    }
                    var viewModel = new ModificarTableroViewModel
                    {
                        NombreDeTablero = tablero.NombreDeTablero,
                        DescripcionDeTablero = tablero.DescripcionDeTablero,
                        IdUsuarioPropietario = tablero.IdUsuarioPropietario
                    };
                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult ConfirmarModificarTablero(ModificarTableroViewModel tableroViewModel)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        var tablero = new Tablero
                        {
                            NombreDeTablero = tableroViewModel.NombreDeTablero,
                            IdUsuarioPropietario = tableroViewModel.IdUsuarioPropietario
                        };
                        if (string.IsNullOrEmpty(tableroViewModel.DescripcionDeTablero))
                        {
                            tablero.DescripcionDeTablero = null; // Establecer explícitamente como null
                        }
                        else
                        {
                            tablero.DescripcionDeTablero = tableroViewModel.DescripcionDeTablero;
                        }
                        tableroRepository.ModificarTablero(tableroViewModel.Id, tablero);
                        return RedirectToAction("MostrarTodosTablero");
                    }
                    return View(tableroViewModel);
                }
                else
                {
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}