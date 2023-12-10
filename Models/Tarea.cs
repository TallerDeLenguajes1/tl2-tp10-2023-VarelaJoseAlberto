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
        private int idTarea;
        private int idTablero;  // Agregar el campo idTablero
        private string? nombreTarea;
        private string? descripcionTarea;
        private string? color;
        private EstadoTarea estadoTarea;
        private int? idUsuarioAsignado;


        public int IdTarea { get => idTarea; set => idTarea = value; }
        public int IdTablero { get => idTablero; set => idTablero = value; } // Propiedad para el idTablero
        public string? NombreTarea { get => nombreTarea; set => nombreTarea = value; }

        public EstadoTarea EstadoTarea { get => estadoTarea; set => estadoTarea = value; }
        public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
        public string? DescripcionTarea { get => descripcionTarea; set => descripcionTarea = value; }
        public string? Color { get => color; set => color = value; }
    }
}
