using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class TareaViewModel
    {
        private int idTarea;
        private int idTablero;
        private string? nombreTarea;
        private EstadoTarea estadoTarea;
        private string? descripcionTarea;
        private string? color;
        private int idUsuarioAsignado;

        public int IdTarea { get => idTarea; set => idTarea = value; }
        public int IdTablero { get => idTablero; set => idTablero = value; }
        public string? NombreTarea { get => nombreTarea; set => nombreTarea = value; }
        public EstadoTarea EstadoTarea { get => estadoTarea; set => estadoTarea = value; }
        public string? DescripcionTarea { get => descripcionTarea; set => descripcionTarea = value; }
        public string? Color { get => color; set => color = value; }
        public int IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }

        public TareaViewModel(Tarea tarea)
        {
            IdTarea = tarea.IdTarea;
            IdTablero = tarea.IdTablero;
            NombreTarea = tarea.NombreTarea;
            EstadoTarea = tarea.EstadoTarea;
            DescripcionTarea = tarea.DescripcionTarea;
            Color = tarea.Color;
            IdUsuarioAsignado = (int)tarea.IdUsuarioAsignado!;
        }
        public TareaViewModel()
        {
        }
    }
}