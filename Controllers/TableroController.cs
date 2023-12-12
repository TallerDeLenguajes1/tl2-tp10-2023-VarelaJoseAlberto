using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class TableroController : Controller
    {
        // private readonly TableroRepository tableroRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly ITableroRepository tableroRepository;
        private readonly ITareaRepository tareaRepository;

        public TableroController(ITableroRepository tableRepository, ILogger<HomeController> logger, ITareaRepository taRepository)
        {
            tableroRepository = tableRepository;
            _logger = logger;
            tareaRepository = taRepository;
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
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        var tablero = tableroRepository.ListarTodosTableros();
                        var tableroVM = tablero.Select(u => new TableroViewModel
                        {
                            IdTablero = u.IdTablero,
                            IdUsuarioPropietario = u.IdUsuarioPropietario,
                            NombreTablero = u.NombreDeTablero,
                            Descripcion = u.DescripcionDeTablero
                        }).ToList();
                        var viewModel = new ListarTablerosViewModel(tableroVM);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        public IActionResult ListarTablerosDeUsuarioEspecifico(int idUsuario)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    var tablero = tableroRepository.ListarTablerosDeUsuarioEspecifico(idUsuario);
                    var tableroVM = tablero.Select(u => new TableroViewModel
                    {
                        IdTablero = u.IdTablero,
                        IdUsuarioPropietario = u.IdUsuarioPropietario,
                        NombreTablero = u.NombreDeTablero,
                        Descripcion = u.DescripcionDeTablero
                    }).ToList();
                    var viewModel = new ListarTablerosViewModel(tableroVM);
                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult AgregarTablero()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarAgregarTablero(CrearTableroViewModel tableroViewModel)
        {
            try
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
                                DescripcionDeTablero = tableroViewModel.DescripcionDeTablero,
                                IdUsuarioPropietario = tableroViewModel.IdUsuarioPropietario
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        public IActionResult EliminarTablero(int idTablero)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        var tablero = tableroRepository.TreaerTableroPorId(idTablero);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        public IActionResult ConfirmarEliminar(Tablero tablero)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult ModificarTablero(int idTablero)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        var tablero = tableroRepository.TreaerTableroPorId(idTablero);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarModificarTablero(ModificarTableroViewModel viewModel)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        if (ModelState.IsValid)
                        {
                            /*  var tablero = tableroRepository.TreaerTableroPorId(viewModel.Id);
                             if (tablero != null)
                             {
                                 tablero.NombreDeTablero = viewModel.NombreDeTablero!;
                                 tablero.IdUsuarioPropietario = viewModel.IdUsuarioPropietario;
                                 tablero.DescripcionDeTablero = viewModel.DescripcionDeTablero;

                                 tableroRepository.ModificarTablero(viewModel.Id, tablero);
                                 return RedirectToAction("MostrarTodosTablero");
                             }
                             else
                             {
                                 return NotFound(); // Tablero no encontrado
                             } */

                            var tablero = new Tablero
                            {
                                NombreDeTablero = viewModel.NombreDeTablero!,
                                IdUsuarioPropietario = viewModel.IdUsuarioPropietario
                            };
                            if (string.IsNullOrEmpty(viewModel.DescripcionDeTablero))
                            {
                                tablero.DescripcionDeTablero = null; // Establecer explícitamente como null
                            }
                            else
                            {
                                tablero.DescripcionDeTablero = viewModel.DescripcionDeTablero;
                            }
                            tableroRepository.ModificarTablero(viewModel.IdTablero, tablero);
                            return RedirectToAction("MostrarTodosTablero");
                        }
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
    }
}