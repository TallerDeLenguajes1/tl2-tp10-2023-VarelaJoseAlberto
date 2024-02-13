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
        private int idUsuarioM;
        private string? nombreDeUsuarioM;
        private string? contraseniaM;
        private Rol rolM;

        public int IdUsuarioM { get => idUsuarioM; set => idUsuarioM = value; }
        public string? NombreDeUsuarioM { get => nombreDeUsuarioM; set => nombreDeUsuarioM = value; }
        public string? ContraseniaM { get => contraseniaM; set => contraseniaM = value; }
        public Rol RolM { get => rolM; set => rolM = value; }

        public Usuario()
        {
        }

        public Usuario(UsuarioViewModel usuarioViewModel)
        {
            IdUsuarioM = usuarioViewModel.IdUsuarioVM;
            NombreDeUsuarioM = usuarioViewModel.NombreDeUsuarioVM!;
            ContraseniaM = usuarioViewModel.ContraseniaVM!;
            RolM = usuarioViewModel.RolVM;
        }

    }
}