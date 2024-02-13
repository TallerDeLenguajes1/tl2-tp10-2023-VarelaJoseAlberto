using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.Models
{
    public enum EstadoTarea
    {
        Ideas,
        ToDo, //hacer
        Doing, //haciendo
        Review, //revisar
        Done //hecho
    }

    public class Tarea
    {
        private int idTareaM;
        private int idTableroM;
        private string? nombreTareaM;
        private string? descripcionTareaM;
        private string? colorM;
        private EstadoTarea estadoTareaM;
        private int? idUsuarioAsignadoM;

        public int IdTareaM { get => idTareaM; set => idTareaM = value; }
        public int IdTableroM { get => idTableroM; set => idTableroM = value; } // Propiedad para el idTablero
        public string? NombreTareaM { get => nombreTareaM; set => nombreTareaM = value; }
        public EstadoTarea EstadoTareaM { get => estadoTareaM; set => estadoTareaM = value; }
        public int? IdUsuarioAsignadoM { get => idUsuarioAsignadoM; set => idUsuarioAsignadoM = value; }
        public string? DescripcionTareaM { get => descripcionTareaM; set => descripcionTareaM = value; }
        public string? ColorM { get => colorM; set => colorM = value; }

        public Tarea()
        {
        }


        public Tarea(TareaViewModel tareaViewModel)
        {
            IdTareaM = tareaViewModel.IdTareaVM;
            IdTableroM = tareaViewModel.IdTableroVM;
            NombreTareaM = tareaViewModel.NombreTareaVM!;
            EstadoTareaM = tareaViewModel.EstadoTareaVM;
            IdUsuarioAsignadoM = tareaViewModel.IdUsuarioAsignadoVM;
            DescripcionTareaM = tareaViewModel.DescripcionTareaVM;
            ColorM = tareaViewModel.ColorVM;
        }
    }
}
