using System.Data.SQLite;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace TP9.Repositorios
{
    public class TableroRepository : ITableroRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

        public void CrearTablero(Tablero nuevoTablero)
        {
            var query = "INSERT INTO Tablero (id_usuario_propietario, nombre_tablero, descripcion_tablero) VALUES (@idPropietario, @nombreTablero, @descripTablero);";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idPropietario", nuevoTablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombreTablero", nuevoTablero.NombreDeTablero));
                command.Parameters.Add(new SQLiteParameter("@descripTablero", nuevoTablero.DescripcionDeTablero));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void EliminarTableroPorId(int idRecibe)
        {
            var query = "DELETE FROM Tablero WHERE id_tablero = @idRecibe";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idRecibe", idRecibe));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<Tablero> ListarTablerosDeUsuarioEspecifico(int idUsuario)
        {
            var query = "SELECT * FROM Tablero WHERE id_usuario_propietario = @idUsuario;";
            List<Tablero> tableros = new List<Tablero>();

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero
                        {
                            IdTablero = Convert.ToInt32(reader["id_tablero"]),
                            NombreDeTablero = reader["nombre_tablero"].ToString(),
                            DescripcionDeTablero = reader["descripcion_tablero"].ToString(),
                            IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"])
                        };
                        tableros.Add(tablero);
                    }
                }
            }

            return tableros;
        }


        public List<Tablero> ListarTodosTableros()
        {
            var query = "SELECT * FROM Tablero";
            List<Tablero> listaDeTablero = new List<Tablero>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tabler = new Tablero();
                        tabler.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tabler.NombreDeTablero = reader["nombre_tablero"].ToString();
                        tabler.DescripcionDeTablero = reader["descripcion_tablero"].ToString();
                        tabler.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                        listaDeTablero.Add(tabler);
                    }
                }
                connection.Close();
            }
            return listaDeTablero;
        }

        public void ModificarTablero(int idRecibe, Tablero modificarTablero)
        {
            var query = "UPDATE Tablero SET id_usuario_propietario = @idPropietario, nombre_tablero = @nombreTablero, descripcion_tablero = @descripTablero WHERE id_tablero = @idRecibe;";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idPropietario", modificarTablero.IdUsuarioPropietario));
                command.Parameters.Add(new SQLiteParameter("@nombreTablero", modificarTablero.NombreDeTablero));
                command.Parameters.Add(new SQLiteParameter("@descripTablero", modificarTablero.DescripcionDeTablero));
                command.Parameters.Add(new SQLiteParameter("@idRecibe", idRecibe));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        public Tablero TreaerTableroPorId(int idTablero)
        {
            var query = "SELECT * FROM Tablero WHERE id_tablero = @idTablero;";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tablero = new Tablero
                        {
                            IdTablero = Convert.ToInt32(reader["id_tablero"]),
                            NombreDeTablero = reader["nombre_tablero"].ToString(),
                            DescripcionDeTablero = reader["descripcion_tablero"].ToString(),
                            IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"])
                        };
                        connection.Close();
                        return tablero;
                    }
                }
            }
            return null; // Devuelve null si no se encuentra el tablero con el ID especificado
        }

    }
}