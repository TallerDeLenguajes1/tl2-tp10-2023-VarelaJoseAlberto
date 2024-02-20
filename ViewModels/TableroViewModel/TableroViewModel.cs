using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class TableroViewModel
    {
        public int IdTableroVM { get; set; }
        public int IdUsuarioPropietarioVM { get; set; }
        public string? NombreTableroVM { get; set; }
        public string? DescripcionVM { get; set; }
        public string? NombreDePropietarioVM { get; set; }

        public TableroViewModel(Tablero tablero)
        {
            IdTableroVM = tablero.IdTableroM;
            IdUsuarioPropietarioVM = tablero.IdUsuarioPropietarioM;
            NombreTableroVM = tablero.NombreDeTableroM;
            DescripcionVM = tablero.DescripcionDeTableroM!;
            NombreDePropietarioVM = tablero.NombreDePropietarioM!;
        }

        public TableroViewModel()
        {
        }
    }
}