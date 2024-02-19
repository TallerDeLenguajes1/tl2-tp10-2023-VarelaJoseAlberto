using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers;

public class UsuarioController : Controller
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<HomeController> _logger;

    public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<HomeController> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (Autorizacion.EstaAutentificado(HttpContext))
        {
            if (Autorizacion.EsAdmin(HttpContext))
            {
                _logger.LogInformation("El usuario con permisos de administrador accedió al Index del controller Usuario.");
                return View();
            }
            else
            {
                _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al Index del controlador de usuarios.");
                return View("AccesoDenegado");
            }
        }
        else
        {
            _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al Index del controlador de usuarios.");
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult CrearUsuario()
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var viewModel = new CrearUsuarioViewModel(new Usuario());
                    _logger.LogInformation("El usuario con permisos de administrador accedió a crear un usuario.");
                    return View(viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al metodo CrearUsuario del controlador de usuarios.");
                    return View("AccesoDenegado");
                }
            }
            else
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al Index del controllador de usuarios.");
                return RedirectToAction("Index", "Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al Crear al Usuario");
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult ConfirmarCrearUsuario(CrearUsuarioViewModel usuarioViewModel)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        var usuario = new Usuario
                        {
                            NombreDeUsuarioM = usuarioViewModel.NombreDeUsuario!,
                            ContraseniaM = usuarioViewModel.Contrasenia!,
                            RolM = usuarioViewModel.Rol
                        };
                        _usuarioRepository.CrearUsuario(usuario);
                        _logger.LogInformation("Se ha creado un nuevo usuario por el administrador.");
                        return RedirectToAction("MostrarTodosUsuarios");
                    }
                    return View(usuarioViewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método ConfirmarCrearUsuario del controlador de usuarios.");
                    return View("AccesoDenegado");
                }
            }
            else
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método ConfirmarCrearUsuario del controlador de usuarios.");
                return RedirectToAction("Index", "Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al Confirmar la Crearcion del Usuario");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EliminarUsuario(int idUsuario)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var usuario = _usuarioRepository.TraerUsuarioPorId(idUsuario);
                    if (usuario == null)
                    {
                        return NotFound();
                    }
                    return View(usuario);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método EliminarUsuario del controlador de usuarios.");
                    return View("AccesoDenegado");
                }
            }
            else
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al Index del controllador de usuarios.");
                return RedirectToAction("Index", "Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al Eliminar el Usuario");
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult ConfirmarEliminar(Usuario user)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    _usuarioRepository.EliminarUsuarioPorId(user.IdUsuarioM);
                    return RedirectToAction("MostrarTodosUsuarios");
                }
                else
                {
                    _logger.LogInformation($"Se ha eliminado el usuario con ID: {user.IdUsuarioM}");
                    return View("AccesoDenegado");
                }
            }
            else
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método ConfirmarEliminar del controlador de usuarios.");
                return RedirectToAction("Index", "Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al Confirmar la Eliminacion del Usuario");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuario)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var usuario = _usuarioRepository.TraerUsuarioPorId(idUsuario);
                    if (usuario == null)
                    {
                        _logger.LogWarning($"No se encontró ningún usuario con el ID: {idUsuario}");
                        View("Error");
                    }
                    var viewModel = new ModificarUsuarioViewModel
                    {
                        NombreDeUsuario = usuario!.NombreDeUsuarioM,
                        Contrasenia = usuario.ContraseniaM,
                        Rol = usuario.RolM
                    };
                    return View(viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método ModificarUsuario del controlador de usuarios.");
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
            _logger.LogError(ex, "Error al Modificar el Usuario");
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult ConfirmarModificarUsuario(ModificarUsuarioViewModel viewModel)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        var usuario = new Usuario
                        {
                            NombreDeUsuarioM = viewModel.NombreDeUsuario!,
                            ContraseniaM = viewModel.Contrasenia!,
                            RolM = viewModel.Rol
                        };
                        _usuarioRepository.ModificarUsuario(viewModel.IdUsuario, usuario);
                        _logger.LogInformation($"Se ha modificado el usuario con ID: {viewModel.IdUsuario}");
                        return RedirectToAction("MostrarTodosUsuarios");
                    }
                    return View(viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método ConfirmarModificarUsuario del controlador de usuarios.");
                    return View("AccesoDenegado");
                }
            }
            else
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método ConfirmarModificarUsuario del controlador de usuarios.");
                return RedirectToAction("Index", "Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al Confirmar La Modificacion del Usuario");
            return BadRequest();
        }
    }

    public IActionResult MostrarTodosUsuarios(string nombreBusqueda)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    List<Usuario> usuarios;

                    if (string.IsNullOrEmpty(nombreBusqueda))
                    {
                        usuarios = _usuarioRepository.TraerTodosUsuarios();
                    }
                    else
                    {
                        usuarios = _usuarioRepository.BuscarUsuarioPorNombre(nombreBusqueda);
                    }

                    var usauarioVM = usuarios.Select(u => new UsuarioViewModel
                    {
                        IdUsuarioVM = u.IdUsuarioM,
                        NombreDeUsuarioVM = u.NombreDeUsuarioM,
                        ContraseniaVM = u.ContraseniaM,
                        RolVM = u.RolM
                    }).ToList();
                    _logger.LogInformation("Mostrando todos los usuarios");
                    var viewModel = new ListarUsuariosViewModel(usauarioVM);
                    return View("MostrarTodosUsuarios", viewModel);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método MostrarTodosUsuarios del controlador de usuarios.");
                    return View("AccesoDenegado");
                }
            }
            else
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método MostrarTodosUsuarios del controlador de usuarios.");
                return RedirectToAction("Index", "Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al mostrar todos los usuarios");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult BuscarUsuarioPorNombre(string nombre)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    return MostrarTodosUsuarios(nombre);
                }
                else
                {
                    _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder al método BuscarUsuarioPorNombre del controlador de usuarios.");
                    return View("AccesoDenegado");
                }
            }
            else
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder sin estar logueado al método BuscarUsuarioPorNombre del controlador de usuarios.");
                return RedirectToAction("Index", "Login");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar usuario por nombre");
            return BadRequest();
        }
    }

    public IActionResult AccesoDenegado()
    {
        try
        {
            if (!Autorizacion.EstaAutentificado(HttpContext))
            {
                _logger.LogWarning("Intento de acceso sin loguearse: Alguien intentó acceder a la página de acceso denegado sin estar logueado.");
                return RedirectToAction("Index", "Login");
            }
            else if (!Autorizacion.EsAdmin(HttpContext))
            {
                _logger.LogWarning("Intento de acceso denegado: Usuario sin permisos de administrador intentó acceder a la página de acceso denegado.");
                return View("AccesoDenegado");
            }
            else
            {
                _logger.LogWarning("Intento de acceso denegado: Administrador intentó acceder a la página de acceso denegado.");
                return View();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al mostrar la página de acceso denegado");
            return BadRequest();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}