using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class TableroController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITableroRepository _tableroRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TableroController(ILogger<HomeController> logger, ITableroRepository tableRepository, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _tableroRepository = tableRepository;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext) || Autorizacion.ObtenerRol(HttpContext) == "operador")
                    {
                        _logger.LogInformation("Acceso exitoso al método Index del controlador de Tableros.");
                        return View();
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado al método Index del controlador de Tableros debido a un rol no autorizado.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método Index del controlador de Tableros. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método Index del controlador de Tableros.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult CrearTablero()
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        var viewModel = new CrearTableroViewModel(new Tablero())
                        {
                            ListadoUsuarios = _usuarioRepository.TraerTodosUsuarios()
                        };
                        _logger.LogInformation("Acceso exitoso al método CrearTablero del controlador de Tableros.");
                        return View(viewModel);
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado al método CrearTablero del controlador de Tableros debido a un rol no autorizado.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método CrearTablero del controlador de Tableros. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método CrearTablero del controlador de Tableros.");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarCrearTablero(CrearTableroViewModel tableroViewModel)
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
                            _logger.LogInformation("Se ha creado exitosamente un nuevo tablero.");
                            return RedirectToAction("MostrarTodosTablero");
                        }
                        _logger.LogWarning("Intento de crear un tablero con datos no válidos.");
                        return RedirectToAction("CrearTablero");
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado al método ConfirmarCrearTablero debido a un rol no autorizado.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método ConfirmarCrearTablero. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método ConfirmarCrearTablero.");
                return BadRequest();
            }
        }

        [HttpGet]
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
                            _logger.LogWarning($"No se encontró ningún tablero con el ID: {idTablero}");
                            return NotFound();
                        }
                        _logger.LogInformation($"Mostrando vista para eliminar el tablero con ID: {idTablero}");
                        return View(tablero);
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado al método EliminarTablero debido a un rol no autorizado.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método EliminarTablero. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método EliminarTablero.");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ConfirmarEliminar(Tablero tablero)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        _tableroRepository.EliminarTableroYTareas(tablero.IdTableroM);
                        _logger.LogInformation($"Tablero con ID {tablero.IdTableroM} y sus tareas asociadas han sido eliminados.");
                        return RedirectToAction("MostrarTodosTablero");
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado al método ConfirmarEliminar debido a un rol no autorizado.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación al método ConfirmarEliminar. Redirigiendo al login.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método ConfirmarEliminar.");
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
                            _logger.LogWarning($"No se encontró ningún tablero con el ID: {idTablero}");
                            return NotFound();
                        }
                        var viewModel = new ModificarTableroViewModel
                        {
                            NombreDeTablero = tablero.NombreDeTableroM!,
                            DescripcionDeTablero = tablero.DescripcionDeTableroM,
                            IdUsuarioPropietario = (int)tablero.IdUsuarioPropietarioM!,
                            ListadoUsuarios = _usuarioRepository.TraerTodosUsuarios()
                        };
                        _logger.LogInformation($"Se ha cargado el tablero con ID: {idTablero} para su modificación.");
                        return View(viewModel);
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método ModificarTablero.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación: Alguien intentó acceder sin estar logueado al método ModificarTablero.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método ModificarTablero.");
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
                            _logger.LogInformation($"Se ha modificado el tablero con ID: {viewModel.IdTablero}");
                            return RedirectToAction("MostrarTodosTablero");
                        }
                        else
                        {
                            _logger.LogWarning("ModelState no es válido en ConfirmarModificarTablero.");
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método ConfirmarModificarTablero.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación: Alguien intentó acceder sin estar logueado al método ConfirmarModificarTablero.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método ConfirmarModificarTablero.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult MostrarTodosTablero(string nombreBusqueda)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        List<Tablero> tableros;
                        if (string.IsNullOrEmpty(nombreBusqueda))
                        {
                            tableros = _tableroRepository.ListarTodosTableros();
                        }
                        else
                        {
                            tableros = _tableroRepository.BuscarTablerosPorNombre(nombreBusqueda);
                        }
                        var tablerosVM = tableros.Select(tablero => new TableroViewModel
                        {
                            IdTableroVM = tablero.IdTableroM,
                            IdUsuarioPropietarioVM = tablero.IdUsuarioPropietarioM,
                            NombreTableroVM = tablero.NombreDeTableroM!,
                            DescripcionVM = tablero.DescripcionDeTableroM,
                            NombreDePropietarioVM = tablero.NombreDePropietarioM!
                        }).ToList();
                        var viewModel = new ListarTablerosViewModel(tablerosVM);
                        _logger.LogInformation("Mostrando todos los tableros.");
                        return View("MostrarTodosTablero", viewModel);
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método MostrarTodosTablero.");
                        return RedirectToAction("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación: Alguien intentó acceder sin estar logueado al método MostrarTodosTablero.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método MostrarTodosTablero.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult ListarTablerosDeUsuarioEspecifico(int idUsuario)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    var tableros = _tableroRepository.ListarTablerosDeUsuarioEspecifico(idUsuario);
                    var tablerosVM = tableros.Select(tablero => new TableroViewModel
                    {
                        IdTableroVM = tablero.IdTableroM,
                        IdUsuarioPropietarioVM = tablero.IdUsuarioPropietarioM,
                        NombreTableroVM = tablero.NombreDeTableroM!,
                        DescripcionVM = tablero.DescripcionDeTableroM,
                        NombreDePropietarioVM = tablero.NombreDePropietarioM!
                    }).ToList();
                    var viewModel = new ListarTablerosViewModel(tablerosVM);
                    _logger.LogInformation($"Mostrando tableros del usuario con ID: {idUsuario}.");
                    return View(viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin autenticación: Alguien intentó acceder sin estar logueado al método ListarTablerosDeUsuarioEspecifico.");
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el método ListarTablerosDeUsuarioEspecifico.");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult BuscarTableroPorNombre(string nombre)
        {
            try
            {
                if (Autorizacion.EstaAutentificado(HttpContext))
                {
                    if (Autorizacion.EsAdmin(HttpContext))
                    {
                        return MostrarTodosTablero(nombre);
                    }
                    else
                    {
                        _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método BuscarTableroPorNombre del controlador de tableros.");
                        return View("AccesoDenegado", "Usuario");
                    }
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método BuscarTableroPorNombre del controlador de tableros.");
                    return RedirectToAction("Index", "Login");
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