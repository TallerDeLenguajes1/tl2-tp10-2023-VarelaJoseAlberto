using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class TableroViewModel
    {
        private int idTablero;
        private int idUsuarioPropietario;
        private string? nombreTablero;
        private string? descripcion;


        public int IdTablero { get => idTablero; set => idTablero = value; }
        public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
        public string? NombreTablero { get => nombreTablero; set => nombreTablero = value; }
        public string? Descripcion { get => descripcion; set => descripcion = value; }

        public TableroViewModel(Tablero tablero)
        {
            IdTablero = tablero.IdTablero;
            IdUsuarioPropietario = tablero.IdUsuarioPropietario;
            NombreTablero = tablero.NombreDeTablero;
            Descripcion = tablero.DescripcionDeTablero!;
        }

        public TableroViewModel()
        {
        }
    }
}