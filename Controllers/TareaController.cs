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

        public TareaController(ILogger<HomeController> logger, ITareaRepository tareaRepository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _tareaRepository = tareaRepository;
            _tableroRepository = tableroRepository;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogInformation("Accediendo al método Index del controlador Tarea.");
                    return View();
                }
                else
                {
                    _logger.LogInformation("Intento de acceso sin autenticación al método Index del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al acceder al método Index del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult CrearTarea()
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    var rol = Autorizacion.ObtenerRol(HttpContext);
                    if (Autorizacion.EsAdmin(HttpContext) || rol == "operador")
                    {
                        var viewModel = new CrearTareaViewModel(new Tarea())
                        {
                            ListadoUsuariosDisponibles = _usuarioRepository.TraerTodosUsuarios(),
                            Tableros = _tableroRepository.ListarTodosTableros()
                        };
                        _logger.LogInformation($"Accediendo al método CrearTarea del controlador Tarea con el rol '{rol}'.");
                        return View(viewModel);
                    }
                    _logger.LogInformation("Intento de acceso denegado al método CrearTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
                else
                {
                    _logger.LogInformation("Intento de acceso sin autenticación al método CrearTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al acceder al método CrearTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarCrearTarea(CrearTareaViewModel tareaViewModel)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método ConfirmarCrearTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        var nuevaTarea = new Tarea
                        {
                            NombreTareaM = tareaViewModel.NombreTarea!,
                            DescripcionTareaM = tareaViewModel.DescripcionTarea,
                            EstadoTareaM = tareaViewModel.EstadoTarea,
                            ColorM = tareaViewModel.ColorTarea,
                            IdTableroM = tareaViewModel.IdTablero,
                            IdUsuarioAsignadoM = tareaViewModel.IdUsuarioAsignado
                        };
                        _tareaRepository.CrearTarea(tareaViewModel.IdTablero, nuevaTarea);
                        _logger.LogInformation("Se ha creado una nueva tarea por el administrador.");
                        return RedirectToAction("MostrarTareas");
                    }
                    else
                    {
                        _logger.LogWarning("El modelo de datos proporcionado no es válido al intentar crear una nueva tarea por el administrador. Redirigiendo al formulario de creación de tarea.");
                        return RedirectToAction("CrearTarea");
                    }
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    var idUsuario = Autorizacion.ObtenerIdUsuario(HttpContext);
                    var tableroIdDeUsuario = _tableroRepository.TreaerTableroPorId(idUsuario);
                    if (tableroIdDeUsuario != null && tableroIdDeUsuario.IdUsuarioPropietarioM == tareaViewModel.IdUsuarioAsignado)
                    {
                        if (ModelState.IsValid)
                        {
                            var nuevaTarea = new Tarea
                            {
                                NombreTareaM = tareaViewModel.NombreTarea!,
                                DescripcionTareaM = tareaViewModel.DescripcionTarea,
                                EstadoTareaM = tareaViewModel.EstadoTarea,
                                ColorM = tareaViewModel.ColorTarea
                            };
                            _tareaRepository.CrearTarea(tableroIdDeUsuario.IdTableroM, nuevaTarea);
                            _logger.LogInformation("Se ha creado una nueva tarea por el operador.");
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            _logger.LogWarning("El modelo de datos proporcionado no es válido al intentar crear una nueva tarea por el operador. Redirigiendo al formulario de creación de tarea.");
                            tareaViewModel.Tableros = _tableroRepository.ListarTodosTableros();
                            return View("CrearTarea", tareaViewModel);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("El operador intentó acceder a un tablero que no le corresponde al crear una nueva tarea. Redirigiendo a AccesoDenegado.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado al método ConfirmarCrearTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar ConfirmarCrearTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult ModificarTarea(int idTarea)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método ModificarTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var tarea = _tareaRepository.ObtenerTareaPorId(idTarea);
                    if (tarea == null)
                    {
                        _logger.LogWarning($"No se encontró ninguna tarea con el ID: {idTarea} al intentar modificarla por el administrador. Redirigiendo a NotFound.");
                        return NotFound();
                    }
                    var viewModel = new ModificarTareaViewModel
                    {
                        NombreTarea = tarea.NombreTareaM,
                        DescripcionTarea = tarea.DescripcionTareaM,
                        EstadoTarea = (EstadoTarea)(int)tarea.EstadoTareaM,
                        ColorTarea = tarea.ColorM,
                        IdUsuarioAsignado = tarea.IdUsuarioAsignadoM,
                        IdTablero = tarea.IdTableroM,
                        ListadoTableros = _tableroRepository.ListarTodosTableros(),
                        ListadoDeUsuarioDisponible = _usuarioRepository.TraerTodosUsuarios()
                    };
                    _logger.LogInformation($"Se mostró el formulario de modificación de tarea por el administrador para la tarea con ID: {idTarea}.");
                    return View(viewModel);
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    var tarea = _tareaRepository.ObtenerTareaPorId(idTarea);
                    if (tarea == null)
                    {
                        _logger.LogWarning($"No se encontró ninguna tarea con el ID: {idTarea} al intentar modificarla por el operador. Redirigiendo a NotFound.");
                        return NotFound();
                    }
                    var viewModel = new ModificarTareaViewModel
                    {
                        NombreTarea = tarea.NombreTareaM,
                        DescripcionTarea = tarea.DescripcionTareaM,
                        EstadoTarea = (EstadoTarea)(int)tarea.EstadoTareaM
                    };
                    _logger.LogInformation($"Se mostró el formulario de modificación de tarea por el operador para la tarea con ID: {idTarea}.");
                    return View(viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado al método ModificarTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar ModificarTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarModificarTarea(ModificarTareaViewModel tareaViewModel)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método ConfirmarModificarTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }

                if (ModelState.IsValid)
                {
                    var tarea = new Tarea
                    {
                        IdTareaM = tareaViewModel.IdTarea,
                        NombreTareaM = tareaViewModel.NombreTarea!,
                        DescripcionTareaM = tareaViewModel.DescripcionTarea,
                        EstadoTareaM = tareaViewModel.EstadoTarea,
                        ColorM = tareaViewModel.ColorTarea,
                        IdUsuarioAsignadoM = tareaViewModel.IdUsuarioAsignado,
                        IdTableroM = tareaViewModel.IdTablero
                    };
                    _tareaRepository.ModificarTarea(tareaViewModel.IdTarea, tarea);
                    _logger.LogInformation($"Se modificó la tarea con ID: {tareaViewModel.IdTarea} correctamente.");
                    return RedirectToAction("MostrarTareas");
                }
                else
                {
                    _logger.LogWarning("ModelState no válido al intentar confirmar la modificación de la tarea. Redirigiendo al Index de Tarea.");
                    return RedirectToAction("Index", "Tarea");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar ConfirmarModificarTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult EliminarTarea(int idTarea)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método EliminarTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var tarea = _tareaRepository.ObtenerTareaPorId(idTarea);
                    if (tarea == null)
                    {
                        _logger.LogWarning($"No se encontró ninguna tarea con el ID: {idTarea} al intentar eliminarla. Redirigiendo a NotFound.");
                        return NotFound();
                    }
                    _logger.LogInformation($"Se mostró la confirmación de eliminación para la tarea con ID: {idTarea}.");
                    return View(tarea);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado al método EliminarTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                    return View("AccesoDenegado");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar EliminarTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarEliminarTarea(Tarea tarea)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método ConfirmarEliminarTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    _tareaRepository.EliminarTarea(tarea.IdTareaM);
                    _logger.LogInformation($"Se eliminó la tarea con ID: {tarea.IdTareaM} correctamente.");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado al método ConfirmarEliminarTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                    return View("AccesoDenegado");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar ConfirmarEliminarTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult MostrarTareas()
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método MostrarTareas del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
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
                        IdUsuarioAsignadoVM = (int)u.IdUsuarioAsignadoM!
                    }).ToList();
                    var viewModel = new ListarTareaViewModel(tareaVM);
                    _logger.LogInformation("Se mostraron todas las tareas correctamente.");
                    return View(viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado al método MostrarTareas del controlador Tarea. Redirigiendo a AccesoDenegado.");
                    return RedirectToAction("AccesoDenegado", "Usuario");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar MostrarTareas del controlador Tarea.");
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
                        IdUsuarioAsignadoVM = (int)u.IdUsuarioAsignadoM!
                    }).ToList();
                    var viewModel = new ListarTareaViewModel(tareaVM);
                    _logger.LogInformation($"Se mostraron todas las tareas del usuario con ID {idUsuario} correctamente.");
                    return View("MostrarTareas", viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método MostrarTareasUsuarioEspecifico del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar MostrarTareasUsuarioEspecifico del controlador Tarea.");
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
                        IdUsuarioAsignadoVM = (int)u.IdUsuarioAsignadoM!
                    }).ToList();
                    var viewModel = new ListarTareaViewModel(tareaVM);
                    _logger.LogInformation($"Se mostraron todas las tareas del tablero con ID {IdTablero} correctamente.");
                    return View("MostrarTareas", viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método MostrarTareasTableroIdEspecifico del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar MostrarTareasTableroIdEspecifico del controlador Tarea.");
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
