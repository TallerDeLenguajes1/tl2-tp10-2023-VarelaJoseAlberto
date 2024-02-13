namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ListarTablerosViewModel
    {
        public List<TableroViewModel> ListaTablerosVM { get; set; }

        public ListarTablerosViewModel(List<TableroViewModel> tablerosVM)
        {
            ListaTablerosVM = tablerosVM;
        }

        public ListarTablerosViewModel()
        {
            ListaTablerosVM = new List<TableroViewModel>();
        }
    }
}