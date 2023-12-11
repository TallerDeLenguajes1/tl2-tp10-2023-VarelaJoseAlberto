using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public interface ITableroRepository
    {
        public void CrearTablero(Tablero nuevoTablero);
        public void ModificarTablero(int idTablero, Tablero modificarTablero);
        public Tablero TreaerTableroPorId(int idTablero);
        public List<Tablero> ListarTodosTableros();
        // lista los tableros de un usuario especifico en la tabla tablero de la BD y recibe un id usuario para comparar con el id_usuario_propietario de la BD
        public List<Tablero> ListarTablerosDeUsuarioEspecifico(int idUsuario);
        public void EliminarTableroPorId(int idTablero);
    }
}