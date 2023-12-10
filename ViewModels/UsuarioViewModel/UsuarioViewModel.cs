using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class UsuarioViewModel
    {
        private int idUsuario;
        private string? nombreDeUsuario;
        private string? contrasenia;
        private Rol rol;


        public string? NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
        public string? Contrasenia { get => contrasenia; set => contrasenia = value; }
        public Rol Rol { get => rol; set => rol = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }

        public UsuarioViewModel(Usuario usuario)
        {
            IdUsuario = usuario.IdUsuario;
            NombreDeUsuario = usuario.NombreDeUsuario!;
            Contrasenia = contrasenia!;
            Rol = usuario.Rol;
        }

        public UsuarioViewModel()
        {
        }
    }
}
