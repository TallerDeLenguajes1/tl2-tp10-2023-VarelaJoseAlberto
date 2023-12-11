namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ListarTareaViewModel
    {
        public List<TareaViewModel>? TareasVM { get; set; }
        public ListarTareaViewModel(List<TareaViewModel> tareasVM)
        {
            TareasVM = tareasVM;
        }
        public ListarTareaViewModel()
        {
            TareasVM = new List<TareaViewModel>();
        }

    }
}