using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ModificarUsuarioViewModel
    {
        public int IdUsuario { get; set; }
        private string? nombreDeUsuario;
        private string? contrasenia;
        private Rol rol;
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Nombre de Usuario")]
        public string? NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Contraseña")]
        public string? Contrasenia { get => contrasenia; set => contrasenia = value; }
        [Required(ErrorMessage = "El rol es requerido.")]
        [Display(Name = "Rol")]
        public Rol Rol { get => rol; set => rol = value; }
        public ModificarUsuarioViewModel()
        {
        }
        public ModificarUsuarioViewModel(Usuario usuario)
        {
            NombreDeUsuario = usuario.NombreDeUsuarioM!;
            Contrasenia = usuario.ContraseniaM!;
            Rol = usuario.RolM;
        }
    }
}
