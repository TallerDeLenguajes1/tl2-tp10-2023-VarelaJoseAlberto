using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{

    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Nombre de Usuario es obligatorio.")]
        // [Display(Name = "Nombre de Usuario")]
        public string? NombreDeUsuario { get; set; }


        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        // [PasswordPropertyText]
        // [Display(Name = "Contraseña")]
        // [MinLength(8, ErrorMessage = "La Contraseña debe tener al menos 8 caracteres.")]

        public string? Contrasenia { get; set; }
    }
}