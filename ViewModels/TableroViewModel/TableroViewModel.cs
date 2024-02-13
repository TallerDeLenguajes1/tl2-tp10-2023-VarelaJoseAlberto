using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class TableroViewModel
    {
        private int idTableroVM;
        private int idUsuarioPropietarioVM;
        private string? nombreTableroVM;
        private string? descripcionVM;


        public int IdTableroVM { get => idTableroVM; set => idTableroVM = value; }
        public int IdUsuarioPropietarioVM { get => idUsuarioPropietarioVM; set => idUsuarioPropietarioVM = value; }
        public string? NombreTableroVM { get => nombreTableroVM; set => nombreTableroVM = value; }
        public string? DescripcionVM { get => descripcionVM; set => descripcionVM = value; }

        public TableroViewModel(Tablero tablero)
        {
            IdTableroVM = tablero.IdTableroM;
            IdUsuarioPropietarioVM = tablero.IdUsuarioPropietarioM;
            NombreTableroVM = tablero.NombreDeTableroM;
            DescripcionVM = tablero.DescripcionDeTableroM!;
        }

        public TableroViewModel()
        {
        }
    }
}