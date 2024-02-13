using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class TareaViewModel
    {
        private int idTareaVM;
        private int idTableroVM;
        private string? nombreTareaVM;
        private EstadoTarea estadoTareaVM;
        private string? descripcionTareaVM;
        private string? colorVM;
        private int idUsuarioAsignadoVM;

        public int IdTareaVM { get => idTareaVM; set => idTareaVM = value; }
        public int IdTableroVM { get => idTableroVM; set => idTableroVM = value; }
        public string? NombreTareaVM { get => nombreTareaVM; set => nombreTareaVM = value; }
        public EstadoTarea EstadoTareaVM { get => estadoTareaVM; set => estadoTareaVM = value; }
        public string? DescripcionTareaVM { get => descripcionTareaVM; set => descripcionTareaVM = value; }
        public string? ColorVM { get => colorVM; set => colorVM = value; }
        public int IdUsuarioAsignadoVM { get => idUsuarioAsignadoVM; set => idUsuarioAsignadoVM = value; }

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