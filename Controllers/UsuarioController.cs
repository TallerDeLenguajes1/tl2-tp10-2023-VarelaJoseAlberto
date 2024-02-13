using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers;

public class UsuarioController : Controller
{
    // private readonly UsuarioRepository usuarioRepository;
    private readonly IUsuarioRepository usuarioRepository;
    private readonly ILogger<HomeController> _logger;

    public UsuarioController(IUsuarioRepository usserRepository, ILogger<HomeController> logger)
    {
        usuarioRepository = usserRepository;
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
            else
            {
                return View("AccesoDenegado");
            }
        }
        else
        {
            // si intenta ingresar forzadamente regresa al usuario
            return RedirectToAction("Index", "Login");
        }
    }

    public IActionResult MostrarTodosUsuarios()
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var usuarios = usuarioRepository.TraerTodosUsuarios();
                    var usauarioVM = usuarios.Select(u => new UsuarioViewModel
                    {
                        IdUsuarioVM = u.IdUsuarioM,
                        NombreDeUsuarioVM = u.NombreDeUsuarioM,
                        RolVM = u.RolM
                    }).ToList();
                    var viewModel = new ListarUsuariosViewModel(usauarioVM);
                    return View(viewModel);
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

    [HttpGet]
    public IActionResult AgregarUsuario()
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var viewModel = new CrearUsuarioViewModel(new Usuario());
                    return View(viewModel);
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
    public IActionResult ConfirmarAgregarUsuario(CrearUsuarioViewModel usuarioViewModel)
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
                        usuarioRepository.CrearUsuario(usuario);
                        return RedirectToAction("MostrarTodosUsuarios");
                    }
                    return View(usuarioViewModel);
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

    public IActionResult EliminarUsuario(int idUsuario)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var usuario = usuarioRepository.TraerUsuarioPorId(idUsuario);
                    if (usuario == null)
                    {
                        return NotFound();
                    }
                    return View(usuario);
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
    public IActionResult ConfirmarEliminar(Usuario user)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    usuarioRepository.EliminarUsuarioPorId(user.IdUsuarioM);
                    return RedirectToAction("MostrarTodosUsuarios");
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

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuario)
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var usuario = usuarioRepository.TraerUsuarioPorId(idUsuario);
                    if (usuario == null)
                    {
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
                        usuarioRepository.ModificarUsuario(viewModel.IdUsuario, usuario);
                        return RedirectToAction("MostrarTodosUsuarios");
                    }
                    return View(viewModel);
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

    public IActionResult AccesoDenegado()
    {
        try
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    return View();
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