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

        [Required(ErrorMessage = "El ID del usuario asignado es requerido.")]
        [Display(Name = "ID del Usuario Asignado")]
        public int? IdUsuarioAsignado { get; set; }


        public int IdTablero { get; set; }

        public List<Tablero>? Tableros { get; set; }



        public CrearTareaViewModel(Tarea tarea)
        {
            NombreTarea = tarea.NombreTareaM;
            DescripcionTarea = tarea.DescripcionTareaM;
            EstadoTarea = tarea.EstadoTareaM;
            ColorTarea = tarea.ColorM;
            if (tarea.IdUsuarioAsignadoM.HasValue)
            {
                IdUsuarioAsignado = (int)tarea.IdUsuarioAsignadoM!; // O el valor por defecto que desees
            }
            else
            {
                IdUsuarioAsignado = 0; // O el valor por defecto que desees
            } // O el valor por defecto que desees
            IdTablero = tarea.IdTableroM;
        }

        public CrearTareaViewModel()
        {
        }
    }
}
