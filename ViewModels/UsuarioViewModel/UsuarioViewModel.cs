using tl2_tp10_2023_VarelaJoseAlberto.Models;
namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class UsuarioViewModel
    {
        public int IdUsuarioVM { get; set; }
        public string? NombreDeUsuarioVM { get; set; }
        public string? ContraseniaVM { get; set; }
        public Rol RolVM { get; set; }

        public UsuarioViewModel() { }
        public UsuarioViewModel(Usuario usuario)
        {
            IdUsuarioVM = usuario.IdUsuarioM;
            NombreDeUsuarioVM = usuario.NombreDeUsuarioM!;
            ContraseniaVM = usuario.ContraseniaM!;
            RolVM = usuario.RolM;
        }
    }
}
