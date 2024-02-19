using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class TableroViewModel
    {
        public int IdTableroVM { get; set; }
        public int? IdUsuarioPropietarioVM { get; set; }
        public string? NombreTableroVM { get; set; }
        public string? DescripcionVM { get; set; }

        public TableroViewModel(Tablero tablero)
        {
            IdTableroVM = tablero.IdTableroM;
            IdUsuarioPropietarioVM = (int)tablero.IdUsuarioPropietarioM!;
            NombreTableroVM = tablero.NombreDeTableroM;
            DescripcionVM = tablero.DescripcionDeTableroM!;
        }

        public TableroViewModel()
        {
        }
    }
}