using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.Repositorios;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{

    public class TareaController : Controller
    {
        private readonly ITareaRepository tareaRepository;
        private readonly ITableroRepository tableroRepository;

        public TareaController(ITareaRepository tarRepository, ITableroRepository tableRepository)
        {
            tableroRepository = tableRepository;
            tareaRepository = tarRepository;
        }

        public IActionResult Index()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var todasLasTareas = tareaRepository.ListarTodasLasTareas();
                    var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                    {
                        IdTablero = u.IdTablero,
                        IdTarea = u.IdTarea,
                        NombreTarea = u.NombreTarea,
                        Color = u.Color,
                        EstadoTarea = u.EstadoTarea,
                        DescripcionTarea = u.DescripcionTarea,
                        IdUsuarioAsignado = u.IdUsuarioAsignado.HasValue ? u.IdUsuarioAsignado.Value : 0
                    }).ToList();
                    var viewModel = new ListarTareaViewModel(tareaVM);
                    return View(viewModel);
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    int idUsuario = Autorizacion.ObtenerIdUsuario(HttpContext);
                    var tareasUsuario = tareaRepository.ListarTareasDeUsuario(idUsuario);
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

        [HttpGet]
        public IActionResult MostrarTareas()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var todasLasTareas = tareaRepository.ListarTodasLasTareas();
                    var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                    {
                        IdTablero = u.IdTablero,
                        IdTarea = u.IdTarea,
                        NombreTarea = u.NombreTarea,
                        Color = u.Color,
                        EstadoTarea = u.EstadoTarea,
                        DescripcionTarea = u.DescripcionTarea,
                        IdUsuarioAsignado = u.IdUsuarioAsignado.HasValue ? u.IdUsuarioAsignado.Value : 0
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


        public IActionResult MostrarTareasTableroEspecifico(int idTablero)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {

                var todasLasTareas = tareaRepository.ListarTareasDeTablero(idTablero);
                var tareaVM = todasLasTareas.Select(u => new TareaViewModel
                {
                    IdTablero = u.IdTablero,
                    IdTarea = u.IdTarea,
                    NombreTarea = u.NombreTarea,
                    Color = u.Color,
                    EstadoTarea = u.EstadoTarea,
                    DescripcionTarea = u.DescripcionTarea,
                    IdUsuarioAsignado = u.IdUsuarioAsignado.HasValue ? u.IdUsuarioAsignado.Value : 0
                }).ToList();
                var viewModel = new ListarTareaViewModel(tareaVM);
                return View(viewModel);

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public IActionResult CrearTarea()
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext) || Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    var viewModel = new CrearTareaViewModel(new Tarea());
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

        [HttpPost]
        public IActionResult ConfirmarCrearTarea(CrearTareaViewModel tareaViewModel)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    if (ModelState.IsValid)
                    {
                        var nuevaTarea = new Tarea
                        {
                            NombreTarea = tareaViewModel.NombreTarea,
                            DescripcionTarea = tareaViewModel.DescripcionTarea,
                            EstadoTarea = (EstadoTarea)tareaViewModel.EstadoTarea,
                            Color = tareaViewModel.ColorTarea
                            // ,
                            // IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado
                        };
                        if (tareaViewModel.IdUsuarioAsignado.HasValue)
                        {
                            nuevaTarea.IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado.Value;
                        }
                        else
                        {
                            // Define un valor predeterminado o maneja el escenario de nulo según tu lógica
                            nuevaTarea.IdUsuarioAsignado = 0; // Por ejemplo, puedes asignar 0
                        }
                        tareaRepository.CrearTarea(tareaViewModel.IdTablero, nuevaTarea);
                        return RedirectToAction("Index");
                    }
                    return View(tareaViewModel);
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    var idUsuario = Autorizacion.ObtenerIdUsuario(HttpContext);
                    var tableroIdDeUsuario = tableroRepository.TreaerTableroPorId(idUsuario);
                    // Verificar si el tablero al que intenta asignar la tarea pertenece al usuario
                    if (tableroIdDeUsuario != null && tableroIdDeUsuario.IdUsuarioPropietario == tareaViewModel.IdUsuarioAsignado)
                    {
                        if (ModelState.IsValid)
                        {
                            var nuevaTarea = new Tarea
                            {
                                NombreTarea = tareaViewModel.NombreTarea,
                                DescripcionTarea = tareaViewModel.DescripcionTarea,
                                EstadoTarea = (EstadoTarea)tareaViewModel.EstadoTarea,
                                Color = tareaViewModel.ColorTarea
                                // ,
                                // IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado
                            };

                            tareaRepository.CrearTarea(tableroIdDeUsuario.IdTablero, nuevaTarea);
                            return RedirectToAction("Index");
                        }
                        return View(tareaViewModel);
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

        public IActionResult ModificarTarea(int id)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var tarea = tareaRepository.ObtenerTareaPorId(id);
                    if (tarea == null)
                    {
                        return NotFound();
                    }
                    var viewModel = new ModificarTareaViewModel
                    {
                        NombreTarea = tarea.NombreTarea,
                        DescripcionTarea = tarea.DescripcionTarea,
                        EstadoTarea = (int)tarea.EstadoTarea,
                        ColorTarea = tarea.Color,
                        // IdUsuarioAsignado = (int)tarea.IdUsuarioAsignado!,
                        IdTablero = tarea.IdTablero
                    };
                    if (tarea.IdUsuarioAsignado.HasValue)
                    {
                        viewModel.IdUsuarioAsignado = tarea.IdUsuarioAsignado.Value;
                    }
                    else
                    {
                        // Define un valor predeterminado o maneja el escenario de nulo según tu lógica
                        viewModel.IdUsuarioAsignado = 0; // Por ejemplo, puedes asignar 0
                    }
                    return View(viewModel);
                }
                else if (Autorizacion.ObtenerRol(HttpContext) == "operador")
                {
                    var tarea = tareaRepository.ObtenerTareaPorId(id);
                    if (tarea == null)
                    {
                        return NotFound();
                    }
                    var viewModel = new ModificarTareaViewModel
                    {
                        NombreTarea = tarea.NombreTarea,
                        DescripcionTarea = tarea.DescripcionTarea,
                        EstadoTarea = (int)tarea.EstadoTarea,

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

        [HttpPost]
        public IActionResult ConfirmarModificarTarea(ModificarTareaViewModel tareaViewModel)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (ModelState.IsValid)
                {
                    var tarea = new Tarea
                    {
                        IdTarea = tareaViewModel.Id,
                        NombreTarea = tareaViewModel.NombreTarea,
                        DescripcionTarea = tareaViewModel.DescripcionTarea,
                        EstadoTarea = (EstadoTarea)tareaViewModel.EstadoTarea,
                        Color = tareaViewModel.ColorTarea,
                        IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado,
                        IdTablero = tareaViewModel.IdTablero
                    };
                    tareaRepository.ModificarTarea(tareaViewModel.Id, tarea);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", "Tarea");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult EliminarTarea(int id)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    var tarea = tareaRepository.ObtenerTareaPorId(id);
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

        [HttpPost]
        public IActionResult ConfirmarEliminarTarea(Tarea tarea)
        {
            if (Autorizacion.EstaAutentificado(HttpContext))
            {
                if (Autorizacion.EsAdmin(HttpContext))
                {
                    tareaRepository.EliminarTarea(tarea.IdTarea);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
