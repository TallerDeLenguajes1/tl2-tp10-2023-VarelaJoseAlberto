using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public interface ITableroRepository
    {
        void CrearTablero(Tablero nuevoTablero);
        void ModificarTablero(int idTablero, Tablero modificarTablero);
        Tablero TreaerTableroPorId(int idTablero);
        List<Tablero> ListarTodosTableros();
        List<Tablero> ListarTablerosDeUsuarioEspecifico(int idUsuario);
        List<Tablero> BuscarTablerosPorNombre(string nombre);
        void EliminarTableroPorId(int idTablero);
        void EliminarTableroYTareas(int idTablero);
    }
}