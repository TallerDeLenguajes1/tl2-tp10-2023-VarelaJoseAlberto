using System.Collections.Generic;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public interface ITareaRepository
    {
        void CrearTarea(int idTablero, Tarea nuevaTarea);
        void ModificarTarea(int idTarea, Tarea tareaModificada);
        Tarea ObtenerTareaPorId(int idTarea); //
        List<Tarea> ListarTareasDeUsuario(int idUsuario);
        List<Tarea> ListarTareasDeTablero(int idTablero);
        List<Tarea> ListarTodasLasTareas();
        List<Tarea> BuscarTareasPorNombre(string nombre);
        void EliminarTarea(int idTarea);
        void AsignarUsuarioATarea(int idUsuario, int idTarea);
    }
}
