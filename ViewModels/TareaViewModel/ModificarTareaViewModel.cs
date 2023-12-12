using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ModificarTareaViewModel
    {
        public int IdTarea { get; set; }

        [Required(ErrorMessage = "El nombre de la tarea es requerido.")]
        [Display(Name = "Nombre de la Tarea")]
        public string? NombreTarea { get; set; }

        [Display(Name = "Descripción de la Tarea")]
        public string? DescripcionTarea { get; set; }

        [Required(ErrorMessage = "El estado de la tarea es requerido.")]
        [Display(Name = "Estado de la Tarea")]
        public int EstadoTarea { get; set; }

        [Required(ErrorMessage = "El color de la tarea es requerido.")]
        [Display(Name = "Color de la Tarea")]
        public string? ColorTarea { get; set; }

        [Required(ErrorMessage = "El ID del usuario asignado es requerido.")]
        [Display(Name = "ID del Usuario Asignado")]
        public int? IdUsuarioAsignado { get; set; }

        [Required(ErrorMessage = "El ID del tablero es requerido.")]
        [Display(Name = "ID del Tablero")]
        public int IdTablero { get; set; }

        public ModificarTareaViewModel()
        {
        }
        public ModificarTareaViewModel(Tarea tarea)
        {
            NombreTarea = tarea.NombreTarea;
            DescripcionTarea = tarea.DescripcionTarea;
            EstadoTarea = (int)tarea.EstadoTarea;
            ColorTarea = tarea.Color;
            IdUsuarioAsignado = (int)tarea.IdUsuarioAsignado!;
            IdTablero = tarea.IdTablero;
        }


    }
}
