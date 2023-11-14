using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using TP9.Repositorios;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers;

public class UsuarioController : Controller
{

    private readonly UsuarioRepository usuarioRepository;

    public UsuarioController()
    {

        usuarioRepository = new UsuarioRepository();
    }

    public IActionResult Index()
    {
        var usuarios = usuarioRepository.TraerTodosUsuarios();
        return View(usuarios);
    }

    public IActionResult MostrarTodosUsuarios()
    {
        var usuarios = usuarioRepository.TraerTodosUsuarios();
        return View(usuarios);
    }

    [HttpGet]
    public IActionResult AgregarUsuario()
    {
        return View(new Usuario());
    }

    [HttpPost]
    public IActionResult AgregarUsuario(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            usuarioRepository.CrearUsuario(usuario);
            return RedirectToAction("MostrarTodosUsuarios");
        }
        return View(usuario);
    }

    public IActionResult EliminarUsuario(int id)
    {
        var usuario = usuarioRepository.TraerUsuarioPorId(id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [HttpPost]
    public IActionResult ConfirmarEliminar(Usuario user)
    {
        usuarioRepository.EliminarUsuarioPorId(user.IdUsuario);
        return RedirectToAction("MostrarTodosUsuarios");
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int id)
    {
        var usuario = usuarioRepository.TraerUsuarioPorId(id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [HttpPost]
    public IActionResult ConfirmarUsuario(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            usuarioRepository.ModificarUsuario(usuario.IdUsuario, usuario);
            return RedirectToAction("MostrarTodosUsuarios");
        }

        return View(usuario);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}



