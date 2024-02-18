using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;
namespace tl2_tp10_2023_VarelaJoseAlberto.Models
{
    public class Tablero
    {
        public int IdTableroM { get; set; }
        public int IdUsuarioPropietarioM { get; set; }
        public string? NombreDeTableroM { get; set; }
        public string? DescripcionDeTableroM { get; set; }

        public Tablero()
        {
        }

        public Tablero(TableroViewModel tableroViewModel)
        {
            IdTableroM = tableroViewModel.IdTableroVM;
            IdUsuarioPropietarioM = tableroViewModel.IdUsuarioPropietarioVM;
            NombreDeTableroM = tableroViewModel.NombreTableroVM!;
            DescripcionDeTableroM = tableroViewModel.DescripcionVM;
        }
    }
}