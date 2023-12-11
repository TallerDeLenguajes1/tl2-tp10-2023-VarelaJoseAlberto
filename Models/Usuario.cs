using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Models
{
    public enum Rol
    {
        admin = 1,
        operador = 2
    }
    public class Usuario
    {
        private int idUsuario;
        private string nombreDeUsuario;
        private string contrasenia;
        private Rol rol;

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
        public string Contrasenia { get => contrasenia; set => contrasenia = value; }
        public Rol Rol { get => rol; set => rol = value; }

        public Usuario()
        {
        }

        public Usuario(UsuarioViewModel usuarioViewModel)
        {
            IdUsuario = usuarioViewModel.IdUsuario;
            NombreDeUsuario = usuarioViewModel.NombreDeUsuario!;
            Contrasenia = usuarioViewModel.Contrasenia!;
            Rol = usuarioViewModel.Rol;
        }

    }
}