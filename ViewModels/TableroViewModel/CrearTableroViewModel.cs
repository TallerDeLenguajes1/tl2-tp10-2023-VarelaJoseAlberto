using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class CrearTableroViewModel
    {
        private string? nombreDeTablero;
        private string? descripcionDeTablero;
        private int idUsuarioPropietario;

        [Required(ErrorMessage = "El nombre del tablero es requerido.")]
        [Display(Name = "Nombre del Tablero")]
        public string? NombreDeTablero { get => nombreDeTablero; set => nombreDeTablero = value; }

        [Display(Name = "Descripción del Tablero")]
        public string? DescripcionDeTablero { get => descripcionDeTablero; set => descripcionDeTablero = value; }

        [Required(ErrorMessage = "El ID del usuario propietario es requerido.")]
        [Display(Name = "ID del Usuario Propietario")]
        public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }

        public CrearTableroViewModel(Tablero tablero)
        {
            NombreDeTablero = tablero.NombreDeTablero;
            DescripcionDeTablero = tablero.DescripcionDeTablero;
            IdUsuarioPropietario = tablero.IdUsuarioPropietario;
        }

        public CrearTableroViewModel() { }
    }
}
