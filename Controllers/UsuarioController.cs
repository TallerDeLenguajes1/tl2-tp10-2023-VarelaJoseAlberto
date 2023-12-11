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

    public UsuarioController(IUsuarioRepository usserRepository)
    {
        usuarioRepository = usserRepository;
    }

    public UsuarioController()
    {
        usuarioRepository = new UsuarioRepository();
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
        if (Autorizacion.EstaAutentificado(HttpContext))
        {
            if (Autorizacion.EsAdmin(HttpContext))
            {
                var usuarios = usuarioRepository.TraerTodosUsuarios();
                var usauarioVM = usuarios.Select(u => new UsuarioViewModel
                {
                    IdUsuario = u.IdUsuario,
                    NombreDeUsuario = u.NombreDeUsuario,
                    Rol = u.Rol
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

    [HttpGet]
    public IActionResult AgregarUsuario()
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

    [HttpPost]
    public IActionResult ConfirmarAgregarUsuario(CrearUsuarioViewModel usuarioViewModel)
    {
        if (Autorizacion.EstaAutentificado(HttpContext))
        {
            if (Autorizacion.EsAdmin(HttpContext))
            {
                if (ModelState.IsValid)
                {
                    var usuario = new Usuario
                    {
                        // Mapear los datos del ViewModel a tu modelo de Usuario
                        NombreDeUsuario = usuarioViewModel.NombreDeUsuario,
                        Contrasenia = usuarioViewModel.Contrasenia,
                        Rol = usuarioViewModel.Rol
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

    public IActionResult EliminarUsuario(int id)
    {
        if (Autorizacion.EstaAutentificado(HttpContext))
        {
            if (Autorizacion.EsAdmin(HttpContext))
            {
                var usuario = usuarioRepository.TraerUsuarioPorId(id);
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

    [HttpPost]
    public IActionResult ConfirmarEliminar(Usuario user)
    {
        if (Autorizacion.EstaAutentificado(HttpContext))
        {
            if (Autorizacion.EsAdmin(HttpContext))
            {
                usuarioRepository.EliminarUsuarioPorId(user.IdUsuario);
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

    [HttpGet]
    public IActionResult ModificarUsuario(int id)
    {
        if (Autorizacion.EstaAutentificado(HttpContext))
        {
            if (Autorizacion.EsAdmin(HttpContext))
            {
                var usuario = usuarioRepository.TraerUsuarioPorId(id);
                if (usuario == null)
                {
                    View("Error");
                }
                var viewModel = new ModificarUsuarioViewModel
                {
                    NombreDeUsuario = usuario!.NombreDeUsuario,
                    Contrasenia = usuario.Contrasenia,
                    Rol = usuario.Rol
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

    [HttpPost]
    public IActionResult ConfirmarModificarUsuario(ModificarUsuarioViewModel viewModel)
    {
        if (Autorizacion.EstaAutentificado(HttpContext))
        {
            if (Autorizacion.EsAdmin(HttpContext))
            {
                if (ModelState.IsValid)
                {
                    var usuario = new Usuario
                    {
                        NombreDeUsuario = viewModel.NombreDeUsuario,
                        Contrasenia = viewModel.Contrasenia,
                        Rol = viewModel.Rol
                    };
                    usuarioRepository.ModificarUsuario(viewModel.Id, usuario);
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

    public IActionResult AccesoDenegado()
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}