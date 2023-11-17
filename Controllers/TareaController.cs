using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using TP9.Repositorios;

namespace TP9.Controllers
{
    public class TareaController : Controller
    {
        private readonly TareaRepository tareaRepository;

        public TareaController()
        {
            tareaRepository = new TareaRepository();
        }

        public IActionResult Index()
        {
            List<Tarea> todasLasTareas = tareaRepository.ListarTodasLasTareas();
            return View(todasLasTareas);
        }


        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Tarea nuevaTarea)
        {
            tareaRepository.CrearTarea(nuevaTarea.IdTablero, nuevaTarea);
            return RedirectToAction("Index");
        }

        public IActionResult Modificar(int id)
        {
            Tarea tarea = tareaRepository.ObtenerTareaPorId(id);
            return View(tarea);
        }

        [HttpPost]
        public IActionResult Modificar(int id, Tarea tareaModificada)
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
        public IActionResult Eliminar(int id, Tarea tareaEliminada)
        {
            tareaRepository.EliminarTarea(id);
            return RedirectToAction("Index");
        }
    }
}
