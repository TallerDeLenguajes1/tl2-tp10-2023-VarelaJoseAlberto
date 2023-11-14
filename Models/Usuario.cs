namespace tl2_tp10_2023_VarelaJoseAlberto.Models
{
    public class Usuario
    {
        private string nombreDeUsuario;
        private int idUsuario;

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    }
}