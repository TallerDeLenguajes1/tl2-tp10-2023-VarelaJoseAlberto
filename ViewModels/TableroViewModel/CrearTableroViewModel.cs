using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class CrearTableroViewModel
    {
        [Required(ErrorMessage = "El nombre del tablero es requerido.")]
        [Display(Name = "Nombre del Tablero")]
        public string? NombreDeTablero { get; set; }

        [Display(Name = "Descripción del Tablero")]
        public string? DescripcionDeTablero { get; set; }

        [Required(ErrorMessage = "El ID del usuario propietario es requerido.")]
        [Display(Name = "ID del Usuario Propietario")]
        public int IdUsuarioPropietario { get; set; }
    }
}
