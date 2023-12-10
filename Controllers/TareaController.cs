using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class TareaController : Controller
    {
        private readonly TareaRepository tareaRepository;
        private readonly TableroRepository tableroRepository;

        public TareaController()
        {
            tableroRepository = new TableroRepository();
            tareaRepository = new TareaRepository();
        }

        public IActionResult Index()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    List<Tarea> todasLasTareas = tareaRepository.ListarTodasLasTareas();
                    return View(todasLasTareas);
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    int idUsuario = Autorizacion.ObtenerIdUsuario(HttpContext);
                    List<Tarea> tareasUsuario = tareaRepository.ListarTareasDeUsuario(idUsuario);
                    return View(tareasUsuario);
                }
                return RedirectToAction("AccesoDenegado", "Usuario");
            }
            else
            {
                // si intenta ingresar forzadamente regresa al usuario
                return RedirectToAction("Index", "Login");
            }
        }


        public IActionResult Crear()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext) || Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    return View();
                }
                return RedirectToAction("AccesoDenegado", "Usuario");
            }
            else
            {
                // si intenta ingresar forzadamente regresa al usuario
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public IActionResult ConfirmarCrear(Tarea nuevaTarea)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        tareaRepository.CrearTarea(nuevaTarea.IdTablero, nuevaTarea);
                        return RedirectToAction("Index");
                    }
                    return View(nuevaTarea);
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    var idUsuario = Autorizacion.ObtenerIdUsuario(HttpContext);
                    var tableroIdDeUsuario = tableroRepository.TreaerTableroPorId(idUsuario);
                    // Verificar si el tablero al que intenta asignar la tarea pertenece al usuario
                    if (tableroIdDeUsuario != null && tableroIdDeUsuario.IdUsuarioPropietario == nuevaTarea.IdUsuarioAsignado)
                    {
                        if (ModelState.IsValid)
                        {
                            tareaRepository.CrearTarea(tableroIdDeUsuario.IdTablero, nuevaTarea);
                            return RedirectToAction("Index");
                        }
                        return View(nuevaTarea);
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






        public IActionResult Modificar(int id)
        {
            Tarea tarea = tareaRepository.ObtenerTareaPorId(id);
            return View(tarea);
        }

        [HttpPost]
        public IActionResult ConfirmarModificar(int id, Tarea tareaModificada)
        {
            tareaRepository.ModificarTarea(id, tareaModificada);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            Tarea tarea = tareaRepository.ObtenerTareaPorId(id);
            return View(tarea);
        }

        [HttpPost]
        public IActionResult ConfirmarEliminar(int id, Tarea tareaEliminada)
        {
            tareaRepository.EliminarTarea(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
