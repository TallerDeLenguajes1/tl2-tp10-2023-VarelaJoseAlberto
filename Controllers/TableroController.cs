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
        private readonly ITableroRepository _tableroRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITareaRepository tareaRepository;

        public TableroController(ITableroRepository tableRepository, ILogger<HomeController> logger, ITareaRepository taRepository, IUsuarioRepository usuarioRepository)
        {
            _tableroRepository = tableRepository;
            _logger = logger;
            tareaRepository = taRepository;
            _usuarioRepository = usuarioRepository;
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
                        var tablero = _tableroRepository.ListarTodosTableros();
                        var tableroVM = tablero.Select(u => new TableroViewModel
                        {
                            IdTableroVM = u.IdTableroM,
                            IdUsuarioPropietarioVM = u.IdUsuarioPropietarioM,
                            NombreTableroVM = u.NombreDeTableroM,
                            DescripcionVM = u.DescripcionDeTableroM
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
                    var tablero = _tableroRepository.ListarTablerosDeUsuarioEspecifico(idUsuario);
                    var tableroVM = tablero.Select(u => new TableroViewModel
                    {
                        IdTableroVM = u.IdTableroM,
                        IdUsuarioPropietarioVM = u.IdUsuarioPropietarioM,
                        NombreTableroVM = u.NombreDeTableroM,
                        DescripcionVM = u.DescripcionDeTableroM
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
                        viewModel.ListadoUsuarios = _usuarioRepository.TraerTodosUsuarios();
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
                                NombreDeTableroM = tableroViewModel.NombreDeTablero,
                                DescripcionDeTableroM = tableroViewModel.DescripcionDeTablero,
                                IdUsuarioPropietarioM = tableroViewModel.IdUsuarioPropietario
                            };
                            _tableroRepository.CrearTablero(tablero);
                            return RedirectToAction("MostrarTodosTablero");
                        }
                        return RedirectToAction("AgregarTAblero");
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
                        var tablero = _tableroRepository.TreaerTableroPorId(idTablero);
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
                        _tableroRepository.EliminarTableroYTareas(tablero.IdTableroM);
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
                        var tablero = _tableroRepository.TreaerTableroPorId(idTablero);
                        if (tablero == null)
                        {
                            return NotFound();
                        }
                        var viewModel = new ModificarTableroViewModel
                        {
                            NombreDeTablero = tablero.NombreDeTableroM,
                            DescripcionDeTablero = tablero.DescripcionDeTableroM,
                            IdUsuarioPropietario = tablero.IdUsuarioPropietarioM,
                            ListadoUsuarios = _usuarioRepository.TraerTodosUsuarios()
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

                            var tablero = new Tablero
                            {
                                NombreDeTableroM = viewModel.NombreDeTablero!,
                                IdUsuarioPropietarioM = viewModel.IdUsuarioPropietario
                            };
                            if (string.IsNullOrEmpty(viewModel.DescripcionDeTablero))
                            {
                                tablero.DescripcionDeTableroM = null; // Establecer explícitamente como null
                            }
                            else
                            {
                                tablero.DescripcionDeTableroM = viewModel.DescripcionDeTablero;
                            }
                            _tableroRepository.ModificarTablero(viewModel.IdTablero, tablero);
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