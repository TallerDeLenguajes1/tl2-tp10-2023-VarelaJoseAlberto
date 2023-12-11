using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Models
{
    public class Tablero
    {
        private int idTablero;
        private int idUsuarioPropietario;
        private string nombreDeTablero;
        private string? descripcionDeTablero;


        public int IdTablero { get => idTablero; set => idTablero = value; }
        public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
        public string NombreDeTablero { get => nombreDeTablero; set => nombreDeTablero = value; }
        public string? DescripcionDeTablero { get => descripcionDeTablero; set => descripcionDeTablero = value; }

        public Tablero()
        {
        }


        public Tablero(TableroViewModel tableroViewModel)
        {
            IdTablero = tableroViewModel.IdTablero;
            IdUsuarioPropietario = tableroViewModel.IdUsuarioPropietario;
            NombreDeTablero = tableroViewModel.NombreTablero!;
            DescripcionDeTablero = tableroViewModel.Descripcion;
        }
    }
}