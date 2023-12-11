namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ListarTablerosViewModel
    {
        public List<TableroViewModel> TablerosVM { get; set; }

        public ListarTablerosViewModel(List<TableroViewModel> tablerosVM)
        {
            TablerosVM = tablerosVM;
        }

        public ListarTablerosViewModel()
        {
            TablerosVM = new List<TableroViewModel>();
        }
    }
}