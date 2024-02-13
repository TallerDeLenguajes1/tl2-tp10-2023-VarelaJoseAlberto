using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public interface ITableroRepository
    {
        void CrearTablero(Tablero nuevoTablero);
        void ModificarTablero(int idTablero, Tablero modificarTablero);
        Tablero TreaerTableroPorId(int idTablero);
        List<Tablero> ListarTodosTableros();
        // lista los tableros de un usuario especifico en la tabla tablero de la BD y recibe un id usuario para comparar con el id_usuario_propietario de la BD
        List<Tablero> ListarTablerosDeUsuarioEspecifico(int idUsuario);
        void EliminarTableroPorId(int idTablero);
        void EliminarTableroYTareas(int idTablero);
    }
}