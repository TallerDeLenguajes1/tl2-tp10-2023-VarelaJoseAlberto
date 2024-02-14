using tl2_tp10_2023_VarelaJoseAlberto.Models;
namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class UsuarioViewModel
    {
        private int idUsuarioVM;
        private string? nombreDeUsuarioVM;
        private string? contraseniaVM;
        private Rol rolVM;
        public string? NombreDeUsuarioVM { get => nombreDeUsuarioVM; set => nombreDeUsuarioVM = value; }
        public string? ContraseniaVM { get => contraseniaVM; set => contraseniaVM = value; }
        public Rol RolVM { get => rolVM; set => rolVM = value; }
        public int IdUsuarioVM { get => idUsuarioVM; set => idUsuarioVM = value; }
        public UsuarioViewModel(Usuario usuario)
        {
            IdUsuarioVM = usuario.IdUsuarioM;
            NombreDeUsuarioVM = usuario.NombreDeUsuarioM!;
            ContraseniaVM = usuario.ContraseniaM!;
            RolVM = usuario.RolM;
        }
        public UsuarioViewModel()
        {
        }
    }
}
