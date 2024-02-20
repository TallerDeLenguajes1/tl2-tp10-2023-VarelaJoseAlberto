using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;
namespace tl2_tp10_2023_VarelaJoseAlberto.Models
{
    public enum EstadoTarea
    {
        Ideas,
        Perdiente,  //ToDo
        EnProceso, //Doing
        Revisar, //Review
        Realizada //Done
    }

    public class Tarea
    {
        public int IdTareaM { get; set; }
        public int IdTableroM { get; set; }
        public string? NombreTareaM { get; set; }
        public EstadoTarea EstadoTareaM { get; set; }
        public int? IdUsuarioAsignadoM { get; set; }
        public string? DescripcionTareaM { get; set; }
        public string? ColorM { get; set; }
        public string? NombreUsuarioAsignadoM { get; set; }
        public string? NombreDelTableroPerteneceM { get; set; }
        public Tarea()
        {
        }

        public Tarea(TareaViewModel tareaViewModel)
        {
            IdTareaM = tareaViewModel.IdTareaVM;
            IdTableroM = tareaViewModel.IdTableroVM;
            NombreTareaM = tareaViewModel.NombreTareaVM;
            EstadoTareaM = tareaViewModel.EstadoTareaVM;
            IdUsuarioAsignadoM = tareaViewModel.IdUsuarioAsignadoVM;
            DescripcionTareaM = tareaViewModel.DescripcionTareaVM;
            ColorM = tareaViewModel.ColorVM;
            NombreUsuarioAsignadoM = tareaViewModel.NombreUsuarioAsignadoVM;
            NombreDelTableroPerteneceM = tareaViewModel.NombreDelTableroPerteneceVM;
        }
    }
}
