using System.Data.SQLite;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public class TableroRepository : ITableroRepository
    {
        // private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";
        private readonly string connectionString;

        public TableroRepository(string CadenaConexion)
        {
            connectionString = CadenaConexion;
        }

        public void CrearTablero(Tablero nuevoTablero)
        {
            var query = "INSERT INTO Tablero (id_usuario_propietario, nombre_tablero, descripcion_tablero) VALUES (@idPropietario, @nombreTablero, @descripTablero);";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idPropietario", nuevoTablero.IdUsuarioPropietario));
                    command.Parameters.Add(new SQLiteParameter("@nombreTablero", nuevoTablero.NombreDeTablero));
                    command.Parameters.Add(new SQLiteParameter("@descripTablero", nuevoTablero.DescripcionDeTablero));
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema en la BD al crear un nuevo Tablero.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public List<Tablero> ListarTodosTableros()
        {
            var query = "SELECT * FROM Tablero";
            List<Tablero> listaDeTablero = new List<Tablero>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tabler = new Tablero();
                            tabler.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                            tabler.NombreDeTablero = reader["nombre_tablero"].ToString()!;
                            tabler.DescripcionDeTablero = reader["descripcion_tablero"].ToString();
                            tabler.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                            listaDeTablero.Add(tabler);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD la lista de Tableros");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (listaDeTablero == null)
            {
                throw new Exception("Lista de Tableros vacia");
            }
            return listaDeTablero;
        }


        public List<Tablero> ListarTablerosDeUsuarioEspecifico(int idRecibe)
        {
            var query = "SELECT * FROM Tablero WHERE id_usuario_propietario = @idUsuario;";
            List<Tablero> tableros = new List<Tablero>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idUsuario", idRecibe));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tablero = new Tablero
                            {
                                IdTablero = Convert.ToInt32(reader["id_tablero"]),
                                NombreDeTablero = reader["nombre_tablero"].ToString()!,
                                DescripcionDeTablero = reader["descripcion_tablero"].ToString(),
                                IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"])
                            };
                            tableros.Add(tablero);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD la lista de Tableros");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (tableros == null)
            {
                throw new Exception("Lista de Tablero vacia");
            }
            return tableros;
        }


        public Tablero TreaerTableroPorId(int idRecibe)
        {
            var query = "SELECT * FROM Tablero WHERE id_tablero = @idTablero;";
            var tablero = new Tablero();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idTablero", idRecibe));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            tablero.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                            tablero.NombreDeTablero = reader["nombre_tablero"].ToString()!;
                            tablero.DescripcionDeTablero = reader["descripcion_tablero"].ToString();
                            tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                        }
                    }

                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD el tablero");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (tablero == null)
            {
                throw new Exception("Tablero no existe");
            }
            return tablero;
        }


        public void EliminarTableroPorId(int idRecibe)
        {
            var query = "DELETE FROM Tablero WHERE id_tablero = @idRecibe";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idRecibe", idRecibe));
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al borrar al Tablero");

                }
                finally
                {
                    connection.Close(); // Asegúrate de cerrar la conexión en el bloque finally
                }
            }
        }


        public void EliminarTableroYTareas(int idTablero)
        {
            string queryEliminarTareas = "DELETE FROM Tarea WHERE id_tablero = @idTablero;";
            string queryEliminarTablero = "DELETE FROM Tablero WHERE id_tablero = @idTablero;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(queryEliminarTareas, connection))
                {
                    command.Parameters.AddWithValue("@idTablero", idTablero);
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand(queryEliminarTablero, connection))
                {
                    command.Parameters.AddWithValue("@idTablero", idTablero);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }




        public void ModificarTablero(int idRecibe, Tablero modificarTablero)
        {
            var query = "UPDATE Tablero SET id_usuario_propietario = @idPropietario, nombre_tablero = @nombreTablero, descripcion_tablero = @descripTablero WHERE id_tablero = @idRecibe;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idPropietario", modificarTablero.IdUsuarioPropietario));
                    command.Parameters.Add(new SQLiteParameter("@nombreTablero", modificarTablero.NombreDeTablero));
                    command.Parameters.Add(new SQLiteParameter("@descripTablero", modificarTablero.DescripcionDeTablero));
                    command.Parameters.Add(new SQLiteParameter("@idRecibe", idRecibe));
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al Modificar el Tablero");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }
}