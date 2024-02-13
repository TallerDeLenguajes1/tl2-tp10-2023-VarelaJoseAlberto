using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Models
{
    public class Tablero
    {
        private int idTableroM;
        private int idUsuarioPropietarioM;
        private string nombreDeTableroM;
        private string? descripcionDeTableroM;


        public int IdTableroM { get => idTableroM; set => idTableroM = value; }
        public int IdUsuarioPropietarioM { get => idUsuarioPropietarioM; set => idUsuarioPropietarioM = value; }
        public string NombreDeTableroM { get => nombreDeTableroM; set => nombreDeTableroM = value; }
        public string? DescripcionDeTableroM { get => descripcionDeTableroM; set => descripcionDeTableroM = value; }

        public Tablero()
        {
        }


        public Tablero(TableroViewModel tableroViewModel)
        {
            IdTableroM = tableroViewModel.IdTablero;
            IdUsuarioPropietarioM = tableroViewModel.IdUsuarioPropietario;
            NombreDeTableroM = tableroViewModel.NombreTablero!;
            DescripcionDeTableroM = tableroViewModel.Descripcion;
        }
    }
}