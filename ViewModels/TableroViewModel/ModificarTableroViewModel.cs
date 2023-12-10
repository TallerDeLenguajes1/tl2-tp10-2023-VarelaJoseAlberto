using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ModificarTableroViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tablero es requerido.")]
        [Display(Name = "Nombre del Tablero")]
        public string? NombreDeTablero { get; set; }

        [Display(Name = "Descripción del Tablero")]
        public string? DescripcionDeTablero { get; set; }

        [Required(ErrorMessage = "El ID del usuario propietario es requerido.")]
        [Display(Name = "ID del Usuario Propietario")]
        public int IdUsuarioPropietario { get; set; }

        public ModificarTableroViewModel()
        {

        }
        public ModificarTableroViewModel(Tablero tablero)
        {
            NombreDeTablero = tablero.NombreDeTablero;
            DescripcionDeTablero = tablero.DescripcionDeTablero;
            IdUsuarioPropietario = tablero.IdUsuarioPropietario;
        }
    }
}
