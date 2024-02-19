using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ModificarTableroViewModel
    {
        public int IdTablero { get; set; }

        [Required(ErrorMessage = "El nombre del tablero es requerido.")]
        [Display(Name = "Nombre del Tablero")]
        public string? NombreDeTablero { get; set; }

        [Display(Name = "Descripción del Tablero")]
        public string? DescripcionDeTablero { get; set; }

        [Required(ErrorMessage = "El nombre del propietario del tablero es requerido.")]
        [Display(Name = "nombre del propietario del Tablero")]
        public int IdUsuarioPropietario { get; set; }
        public List<Usuario>? ListadoUsuarios { get; set; }

        public ModificarTableroViewModel()
        {
        }
        public ModificarTableroViewModel(Tablero tablero)
        {
            NombreDeTablero = tablero.NombreDeTableroM;
            DescripcionDeTablero = tablero.DescripcionDeTableroM;
            IdUsuarioPropietario = (int)tablero.IdUsuarioPropietarioM!;
        }
    }
}
