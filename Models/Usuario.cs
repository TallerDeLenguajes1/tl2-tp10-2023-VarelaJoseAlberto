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
        public int IdUsuarioM { get; set; }
        public string? NombreDeUsuarioM { get; set; }
        public string? ContraseniaM { get; set; }
        public Rol RolM { get; set; }
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