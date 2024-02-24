using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class CrearTableroViewModel
    {
        [Required(ErrorMessage = "El nombre del tablero es requerido.")]
        [Display(Name = "Nombre del Tablero")]
        public string? NombreDeTablero { get; set; }

        [Display(Name = "Descripción del Tablero")]
        public string? DescripcionDeTablero { get; set; }

        [Required(ErrorMessage = "El propietario del tablero es requerido.")]
        [Display(Name = "Nombre del propietario del Tablero")]
        public int IdUsuarioPropietario { get; set; }
        public List<Usuario>? ListadoUsuarios { get; set; }

        public CrearTableroViewModel() { }
        public CrearTableroViewModel(Tablero tablero)
        {
            NombreDeTablero = tablero.NombreDeTableroM!;
            DescripcionDeTablero = tablero.DescripcionDeTableroM;
            IdUsuarioPropietario = tablero.IdUsuarioPropietarioM!;
        }
    }
}
