using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class CrearTareaViewModel
    {
        [Required(ErrorMessage = "El nombre de la tarea es requerido.")]
        [Display(Name = "Nombre de la Tarea")]
        public string? NombreTarea { get; set; }

        [Display(Name = "Descripción de la Tarea")]
        public string? DescripcionTarea { get; set; }

        [Required(ErrorMessage = "El estado de la tarea es requerido.")]
        [Display(Name = "Estado de la Tarea")]
        public EstadoTarea EstadoTarea { get; set; }

        [Required(ErrorMessage = "El color de la tarea es requerido.")]
        [Display(Name = "Color de la Tarea")]
        public string? ColorTarea { get; set; }

        public int IdTablero { get; set; }
        public List<Tablero>? ListadoTableros { get; set; }

        public int? IdUsuarioAsignado { get; set; }
        public List<Usuario>? ListadoUsuariosDisponibles { get; set; }



        public CrearTareaViewModel(Tarea tarea)
        {
            NombreTarea = tarea.NombreTareaM;
            DescripcionTarea = tarea.DescripcionTareaM;
            EstadoTarea = tarea.EstadoTareaM;
            ColorTarea = tarea.ColorM;
            IdUsuarioAsignado = tarea.IdUsuarioAsignadoM!; // O el valor por defecto que desees
            IdTablero = tarea.IdTableroM;
        }

        public CrearTareaViewModel()
        {
        }
    }
}
