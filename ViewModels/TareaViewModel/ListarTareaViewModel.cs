namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ListarTareaViewModel
    {
        public List<TareaViewModel>? ListaTareasVM { get; set; }
        public ListarTareaViewModel(List<TareaViewModel> tareasVM)
        {
            ListaTareasVM = tareasVM;
        }
        public ListarTareaViewModel()
        {
            ListaTareasVM = new List<TareaViewModel>();
        }

    }
}