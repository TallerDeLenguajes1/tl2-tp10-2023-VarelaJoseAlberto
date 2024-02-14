using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class TareaViewModel
    {
        public int IdTareaVM { get; set; }
        public int IdTableroVM { get; set; }
        public string? NombreTareaVM { get; set; }
        public EstadoTarea EstadoTareaVM { get; set; }
        public string? DescripcionTareaVM { get; set; }
        public string? ColorVM { get; set; }
        public int IdUsuarioAsignadoVM { get; set; }

        public TareaViewModel(Tarea tarea)
        {
            IdTareaVM = tarea.IdTareaM;
            IdTableroVM = tarea.IdTableroM;
            NombreTareaVM = tarea.NombreTareaM;
            EstadoTareaVM = tarea.EstadoTareaM;
            DescripcionTareaVM = tarea.DescripcionTareaM;
            ColorVM = tarea.ColorM;
            IdUsuarioAsignadoVM = (int)tarea.IdUsuarioAsignadoM!;
        }
        public TareaViewModel()
        {
        }
    }
}