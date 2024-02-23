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
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogInformation("Intento de acceso sin autenticación al método CrearTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                var rol = Autorizacion.ObtenerRol(HttpContext);
                if (Autorizacion.EsAdmin(HttpContext) || rol == "operador")
                {
                    var viewModel = new CrearTareaViewModel()
                    {
                        ListadoUsuariosDisponibles = _usuarioRepository.TraerTodosUsuarios(),
                        ListadoTableros = _tableroRepository.ListarTodosTableros()
                    };
                    _logger.LogInformation($"Accediendo al método CrearTarea del controlador Tarea con el rol '{rol}'.");
                    return View(viewModel);
                }
                _logger.LogInformation("Intento de acceso denegado al método CrearTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                return RedirectToAction("AccesoDenegado", "Usuario");
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
        public IActionResult CrearTareaXId(int idTablero)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogInformation("Intento de acceso sin autenticación al método CrearTarea del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                var rol = Autorizacion.ObtenerRol(HttpContext);
                if (rol == "admin" || rol == "operador")
                {
                    var viewModel = new CrearTareaViewModel(new Tarea())
                    {
                        ListadoUsuariosDisponibles = _usuarioRepository.TraerTodosUsuarios(),
                        IdTablero = idTablero
                    };
                    _logger.LogInformation($"Accediendo al método CrearTareaXId del controlador Tarea con el rol '{rol}'.");
                    return View(viewModel);
                }
                _logger.LogInformation("Intento de acceso denegado al método CrearTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                return RedirectToAction("AccesoDenegado", "Usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al acceder al método CrearTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarCrearTareaXId(CrearTareaViewModel tareaViewModel)
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
                    if (ModelState.IsValid)
                    {
                        var nuevaTarea = new Tarea
                        {
                            NombreTareaM = tareaViewModel.NombreTarea!,
                            DescripcionTareaM = tareaViewModel.DescripcionTarea,
                            EstadoTareaM = tareaViewModel.EstadoTarea,
                            ColorM = tareaViewModel.ColorTarea,
                        };
                        _tareaRepository.CrearTarea(tareaViewModel.IdTablero, nuevaTarea);
                        _logger.LogInformation("Se ha creado una nueva tarea por el operador.");
                        return RedirectToAction("MostrarTareasTableroIdEspecifico", new { idTablero = tareaViewModel.IdTablero });
                    }
                    else
                    {
                        _logger.LogWarning("El modelo de datos proporcionado no es válido al intentar crear una nueva tarea por el operador. Redirigiendo al formulario de creación de tarea.");
                        tareaViewModel.ListadoTableros = _tableroRepository.ListarTodosTableros();
                        return View("CrearTareaXId", tareaViewModel);
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
                _logger.LogError(ex, "Error al ejecutar ConfirmarCrearTareaXId del controlador Tarea.");
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
                        IdTarea = tarea.IdTareaM,
                        NombreTarea = tarea.NombreTareaM,
                        DescripcionTarea = tarea.DescripcionTareaM,
                        EstadoTarea = (EstadoTarea)(int)tarea.EstadoTareaM,
                        IdUsuarioAsignado = tarea.IdUsuarioAsignadoM,
                        ColorTarea = tarea.ColorM,
                        IdTablero = tarea.IdTableroM,
                        ListadoDeUsuarioDisponible = _usuarioRepository.TraerTodosUsuarios()
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
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        return RedirectToAction("MostrarTareas");
                    }
                    else
                    {
                        return RedirectToAction("MostrarTareasTableroIdEspecifico", new { idTablero = tarea.IdTableroM });
                    }
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
                var tarea = _tareaRepository.ObtenerTareaPorId(idTarea);
                if (tarea == null)
                {
                    _logger.LogWarning($"No se encontró ninguna tarea con el ID: {idTarea} al intentar eliminarla. Redirigiendo a NotFound.");
                    return NotFound();
                }
                _logger.LogInformation($"Se mostró la confirmación de eliminación para la tarea con ID: {idTarea}.");
                return View(tarea);

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
                int idTab = tarea.IdTableroM;
                _logger.LogInformation($"Se eliminó la tarea con ID: {tarea.IdTareaM} correctamente.");
                _tareaRepository.EliminarTarea(tarea.IdTareaM);
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    return RedirectToAction("MostrarTareas");
                }
                else
                {
                    // _logger.LogWarning("Intento de acceso denegado al método ConfirmarEliminarTarea del controlador Tarea. Redirigiendo a AccesoDenegado.");
                    // return View("AccesoDenegado");
                    return RedirectToAction("MostrarTareasTableroIdEspecifico", new { idTablero = idTab });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar ConfirmarEliminarTarea del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult MostrarTareas(string nombreBusqueda)
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
                    List<Tarea> tareas;
                    if (string.IsNullOrEmpty(nombreBusqueda))
                    {
                        tareas = _tareaRepository.ListarTodasLasTareas();
                    }
                    else
                    {
                        tareas = _tareaRepository.BuscarTareasPorNombre(nombreBusqueda);
                    }
                    var tareaVM = tareas.Select(u => new TareaViewModel
                    {
                        IdTableroVM = u.IdTableroM,
                        IdTareaVM = u.IdTareaM,
                        NombreTareaVM = u.NombreTareaM,
                        ColorVM = u.ColorM,
                        EstadoTareaVM = u.EstadoTareaM,
                        DescripcionTareaVM = u.DescripcionTareaM,
                        IdUsuarioAsignadoVM = u.IdUsuarioAsignadoM.HasValue ? u.IdUsuarioAsignadoM.Value : 0,
                        NombreUsuarioAsignadoVM = u.NombreUsuarioAsignadoM,
                        NombreDelTableroPerteneceVM = u.NombreDelTableroPerteneceM
                    }).ToList();
                    var viewModel = new ListarTareaViewModel(tareaVM);
                    _logger.LogInformation("Se mostraron todas las tareas correctamente.");
                    return View("MostrarTareas", viewModel);
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
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método MostrarTareasUsuarioEspecifico del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                var todasLasTareas = _tareaRepository.ListarTareasDeUsuario(idUsuario);
                var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                {
                    IdTableroVM = u.IdTableroM,
                    IdTareaVM = u.IdTareaM,
                    NombreTareaVM = u.NombreTareaM,
                    ColorVM = u.ColorM,
                    EstadoTareaVM = u.EstadoTareaM,
                    DescripcionTareaVM = u.DescripcionTareaM,
                    NombreDelTableroPerteneceVM = u.NombreDelTableroPerteneceM
                }).ToList();
                var viewModel = new ListarTareaViewModel(tareaVM);
                _logger.LogInformation($"Se mostraron todas las tareas del usuario con ID {idUsuario} correctamente.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar MostrarTareasUsuarioEspecifico del controlador Tarea.");
                return BadRequest();
            }
        }

        public IActionResult MostrarTareasTableroIdEspecifico(int idTablero)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método MostrarTareasTableroIdEspecifico del controlador Tarea. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
                var todasLasTareas = _tareaRepository.ListarTareasDeTablero(idTablero);
                var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                {
                    IdTableroVM = u.IdTableroM,
                    IdTareaVM = u.IdTareaM,
                    NombreTareaVM = u.NombreTareaM,
                    ColorVM = u.ColorM,
                    EstadoTareaVM = u.EstadoTareaM,
                    DescripcionTareaVM = u.DescripcionTareaM,
                    IdUsuarioAsignadoVM = u.IdUsuarioAsignadoM.HasValue ? u.IdUsuarioAsignadoM.Value : 0,
                    NombreUsuarioAsignadoVM = u.NombreUsuarioAsignadoM,
                    NombreDelTableroPerteneceVM = u.NombreDelTableroPerteneceM
                }).ToList();
                var viewModel = new ListarTareaViewModel(tareaVM);
                _logger.LogInformation($"Se mostraron todas las tareas del tablero con ID {idTablero} correctamente.");
                return View("MostrarTareasTableroIdEspecifico", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar MostrarTareasTableroIdEspecifico del controlador Tarea.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult BuscarTareasPorNombre(string nombre)
        {
            try
            {
                if (!Autorizacion.EstaAutentificado(HttpContext))
                {
                    _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método BuscarTareasPorNombre del controlador de tarea.");
                    return RedirectToAction("Index", "Login");
                }
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    return MostrarTareas(nombre);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método BuscarTareasPorNombre del controlador de tarea.");
                    return View("AccesoDenegado");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar tableros por nombre");
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