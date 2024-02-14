using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class TareaController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITareaRepository _tareaRepository;
        private readonly ITableroRepository _tableroRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TareaController(ITareaRepository tareaRepository, ITableroRepository tableroRepository, ILogger<HomeController> logger, IUsuarioRepository usuarioRepository)
        {
            _tableroRepository = tableroRepository;
            _tareaRepository = tareaRepository;
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                return View();
            }
            else
            {
                // si intenta ingresar forzadamente regresa al usuario
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public IActionResult MostrarTareas()
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        var todasLasTareas = _tareaRepository.ListarTodasLasTareas();
                        var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                        {
                            IdTableroVM = u.IdTableroM,
                            IdTareaVM = u.IdTareaM,
                            NombreTareaVM = u.NombreTareaM,
                            ColorVM = u.ColorM,
                            EstadoTareaVM = u.EstadoTareaM,
                            DescripcionTareaVM = u.DescripcionTareaM,
                            IdUsuarioAsignadoVM = u.IdUsuarioAsignadoM.HasValue ? u.IdUsuarioAsignadoM.Value : 0
                        }).ToList();
                        var viewModel = new ListarTareaViewModel(tareaVM);
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


        [HttpGet]
        public IActionResult MostrarTareasUsuarioEspecifico(int idUsuario)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {

                    var todasLasTareas = _tareaRepository.ListarTareasDeUsuario(idUsuario);
                    var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                    {
                        IdTableroVM = u.IdTableroM,
                        IdTareaVM = u.IdTareaM,
                        NombreTareaVM = u.NombreTareaM,
                        ColorVM = u.ColorM,
                        EstadoTareaVM = u.EstadoTareaM,
                        DescripcionTareaVM = u.DescripcionTareaM,
                        IdUsuarioAsignadoVM = u.IdUsuarioAsignadoM.HasValue ? u.IdUsuarioAsignadoM.Value : 0
                    }).ToList();
                    var viewModel = new ListarTareaViewModel(tareaVM);
                    return View("MostrarTareas", viewModel);

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


        public IActionResult MostrarTareasTableroIdEspecifico(int IdTablero)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    var idTableroSeleccionado = IdTablero;
                    var todasLasTareas = _tareaRepository.ListarTareasDeTablero(idTableroSeleccionado);
                    var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                    {
                        IdTableroVM = u.IdTableroM,
                        IdTareaVM = u.IdTareaM,
                        NombreTareaVM = u.NombreTareaM,
                        ColorVM = u.ColorM,
                        EstadoTareaVM = u.EstadoTareaM,
                        DescripcionTareaVM = u.DescripcionTareaM,
                        IdUsuarioAsignadoVM = u.IdUsuarioAsignadoM.HasValue ? u.IdUsuarioAsignadoM.Value : 0
                    }).ToList();
                    var viewModel = new ListarTareaViewModel(tareaVM);
                    return View("MostrarTareas", viewModel);

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

        public IActionResult CrearTarea()
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext) || Autorizacion.ObtenerRol(HttpContext) == "operador")
                    {
                        var viewModel = new CrearTareaViewModel(new Tarea());
                        viewModel.ListadoUsuariosDisponibles = _usuarioRepository.TraerTodosUsuarios();
                        viewModel.Tableros = _tableroRepository.ListarTodosTableros();
                        return View(viewModel);
                    }
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
                else
                {
                    // si intenta ingresar forzadamente regresa al usuario
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
        public IActionResult ConfirmarCrearTarea(CrearTareaViewModel tareaViewModel)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        if (ModelState.IsValid)
                        {
                            var nuevaTarea = new Tarea
                            {
                                NombreTareaM = tareaViewModel.NombreTarea!,
                                DescripcionTareaM = tareaViewModel.DescripcionTarea,
                                EstadoTareaM = (EstadoTarea)tareaViewModel.EstadoTarea,
                                ColorM = tareaViewModel.ColorTarea,
                                IdTableroM = tareaViewModel.IdTablero
                                // ,
                                // IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado
                            };
                            if (tareaViewModel.IdUsuarioAsignado.HasValue)
                            {
                                nuevaTarea.IdUsuarioAsignadoM = tareaViewModel.IdUsuarioAsignado.Value;
                            }
                            else
                            {
                                // Define un valor predeterminado o maneja el escenario de nulo según tu lógica
                                nuevaTarea.IdUsuarioAsignadoM = 0; // Por ejemplo, puedes asignar 0
                            }
                            _tareaRepository.CrearTarea(tareaViewModel.IdTablero, nuevaTarea);
                            return RedirectToAction("MostrarTareas");
                        }
                        else
                        {
                            return RedirectToAction("CrearTarea");
                        }
                        // return View(tareaViewModel);
                    }
                    else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                    {
                        var idUsuario = Autorizacion.ObtenerIdUsuario(HttpContext);
                        var tableroIdDeUsuario = _tableroRepository.TreaerTableroPorId(idUsuario);
                        // Verificar si el tablero al que intenta asignar la tarea pertenece al usuario
                        if (tableroIdDeUsuario != null && tableroIdDeUsuario.IdUsuarioPropietarioM == tareaViewModel.IdUsuarioAsignado)
                        {
                            if (ModelState.IsValid)
                            {
                                var nuevaTarea = new Tarea
                                {
                                    NombreTareaM = tareaViewModel.NombreTarea!,
                                    DescripcionTareaM = tareaViewModel.DescripcionTarea,
                                    EstadoTareaM = (EstadoTarea)tareaViewModel.EstadoTarea,
                                    ColorM = tareaViewModel.ColorTarea
                                    // ,
                                    // IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado
                                };

                                _tareaRepository.CrearTarea(tableroIdDeUsuario.IdTableroM, nuevaTarea);
                                return RedirectToAction("Index");
                            }
                            tareaViewModel.Tableros = _tableroRepository.ListarTodosTableros();
                            return View("CrearTarea", tareaViewModel);
                        }
                        // Si no es el tablero del usuario, redireccionar o mostrar mensaje de error
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                    return RedirectToAction("AccesoDenegado", "Usuario");
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

        public IActionResult ModificarTarea(int idTarea)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        var tarea = _tareaRepository.ObtenerTareaPorId(idTarea);
                        if (tarea == null)
                        {
                            return NotFound();
                        }
                        var viewModel = new ModificarTareaViewModel
                        {
                            NombreTarea = tarea.NombreTareaM,
                            DescripcionTarea = tarea.DescripcionTareaM,
                            EstadoTarea = (EstadoTarea)(int)tarea.EstadoTareaM,
                            ColorTarea = tarea.ColorM,
                            // IdUsuarioAsignado = (int)tarea.IdUsuarioAsignado!,
                            IdTablero = tarea.IdTableroM,
                            Tableros = _tableroRepository.ListarTodosTableros()
                        };
                        viewModel.ListadoDeUsuarioDisponible = _usuarioRepository.TraerTodosUsuarios();
                        return View(viewModel);
                    }
                    else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                    {
                        var tarea = _tareaRepository.ObtenerTareaPorId(idTarea);
                        if (tarea == null)
                        {
                            return NotFound();
                        }
                        var viewModel = new ModificarTareaViewModel
                        {
                            NombreTarea = tarea.NombreTareaM,
                            DescripcionTarea = tarea.DescripcionTareaM,
                            EstadoTarea = (EstadoTarea)(int)tarea.EstadoTareaM,

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
        public IActionResult ConfirmarModificarTarea(ModificarTareaViewModel tareaViewModel)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        var tarea = new Tarea
                        {
                            IdTareaM = tareaViewModel.IdTarea,
                            NombreTareaM = tareaViewModel.NombreTarea!,
                            DescripcionTareaM = tareaViewModel.DescripcionTarea,
                            EstadoTareaM = (EstadoTarea)tareaViewModel.EstadoTarea,
                            ColorM = tareaViewModel.ColorTarea,
                            IdUsuarioAsignadoM = tareaViewModel.IdUsuarioAsignado,
                            IdTableroM = tareaViewModel.IdTablero
                        };
                        _tareaRepository.ModificarTarea(tareaViewModel.IdTarea, tarea);
                        return RedirectToAction("MostrarTareas");
                    }
                    return RedirectToAction("Index", "Tarea");
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

        public IActionResult EliminarTarea(int id)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        var tarea = _tareaRepository.ObtenerTareaPorId(id);
                        if (tarea == null)
                        {
                            return NotFound();
                        }
                        return View(tarea);
                    }
                    else
                    {
                        return View("AccesoDenegado");
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
        public IActionResult ConfirmarEliminarTarea(Tarea tarea)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        _tareaRepository.EliminarTarea(tarea.IdTareaM);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("AccesoDenegado");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
