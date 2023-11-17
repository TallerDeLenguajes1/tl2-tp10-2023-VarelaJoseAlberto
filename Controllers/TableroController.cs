using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using TP9.Repositorios;

namespace tl2_tp10_2023_VarelaJoseAlberto.Controllers
{
    public class TableroController : Controller
    {
        private readonly TableroRepository tableroRepository;
        public TableroController()
        {
            tableroRepository = new TableroRepository();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MostrarTodosTablero()
        {
            var tablero = tableroRepository.ListarTodosTableros();
            return View(tablero);
        }

        public IActionResult Buscar()
        {
            return View();
        }
        public IActionResult ListarTablerosDeUsuarioEspecifico(int idUsuario)
        {
            var tablero = tableroRepository.ListarTablerosDeUsuarioEspecifico(idUsuario);
            return View(tablero);
        }

        [HttpGet]
        public IActionResult AgregarTablero()
        {
            return View(new Tablero());
        }

        [HttpPost]
        public IActionResult AgregarTablero(Tablero tablero)
        {
            if (ModelState.IsValid)
            {
                tableroRepository.CrearTablero(tablero);
                return RedirectToAction("MostrarTodosTablero");
            }
            return View(tablero);
        }

        public IActionResult EliminarTablero(int id)
        {
            var tablero = tableroRepository.TreaerTableroPorId(id);
            if (tablero == null)
            {
                return NotFound();
            }
            return View(tablero);
        }

        public IActionResult ConfirmarEliminar(Tablero tablero)
        {
            tableroRepository.EliminarTableroPorId(tablero.IdTablero);
            return RedirectToAction("MostrarTodosTablero");
        }

        [HttpGet]
        public IActionResult ModificarTablero(int id)
        {
            var tablero = tableroRepository.TreaerTableroPorId(id);
            if (tablero == null)
            {
                return NotFound();
            }
            return View(tablero);
        }

        [HttpPost]
        public IActionResult ConfirmarTablero(Tablero tablero)
        {
            if (ModelState.IsValid)
            {
                tableroRepository.ModificarTablero(tablero.IdTablero, tablero);
                return RedirectToAction("MostrarTodosTablero");
            }
            return View(tablero);
        }
    }
}