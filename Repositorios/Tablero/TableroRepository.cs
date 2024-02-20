using System.Data.SQLite;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public class TableroRepository : ITableroRepository
    {
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
                    command.Parameters.Add(new SQLiteParameter("@idPropietario", nuevoTablero.IdUsuarioPropietarioM));
                    command.Parameters.Add(new SQLiteParameter("@nombreTablero", nuevoTablero.NombreDeTableroM));
                    command.Parameters.Add(new SQLiteParameter("@descripTablero", nuevoTablero.DescripcionDeTableroM));
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
            var query = "SELECT * FROM Tablero INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id_usuario";
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
                            var tabler = new Tablero
                            {
                                IdTableroM = Convert.ToInt32(reader["id_tablero"]),
                                NombreDeTableroM = reader["nombre_tablero"].ToString()!,
                                DescripcionDeTableroM = reader["descripcion_tablero"].ToString(),
                                IdUsuarioPropietarioM = Convert.ToInt32(reader["id_usuario_propietario"]),
                                NombreDePropietarioM = reader["nombre_de_usuario"].ToString()!
                            };
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
            var query = "SELECT * FROM Tablero INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id_usuario " +
            "WHERE id_usuario_propietario = @idUsuario;";
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
                                IdTableroM = Convert.ToInt32(reader["id_tablero"]),
                                NombreDeTableroM = reader["nombre_tablero"].ToString()!,
                                DescripcionDeTableroM = reader["descripcion_tablero"].ToString(),
                                IdUsuarioPropietarioM = Convert.ToInt32(reader["id_usuario_propietario"]),
                                NombreDePropietarioM = reader["nombre_de_usuario"].ToString()!
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
            var query = "SELECT * FROM Tablero INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id_usuario WHERE id_tablero = @idTablero;";
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
                            tablero.IdTableroM = Convert.ToInt32(reader["id_tablero"]);
                            tablero.NombreDeTableroM = reader["nombre_tablero"].ToString()!;
                            tablero.DescripcionDeTableroM = reader["descripcion_tablero"].ToString();
                            tablero.IdUsuarioPropietarioM = Convert.ToInt32(reader["id_usuario_propietario"]);
                            tablero.NombreDePropietarioM = reader["nombre_de_usuario"].ToString()!;
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

        public void EliminarTableroPorId(int idTablero)
        {
            var query = "DELETE FROM Tablero WHERE id_tablero = @idTablero";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
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
            var query = "UPDATE Tablero SET id_usuario_propietario = @idPropietario, nombre_tablero = @nombreTablero, " +
            "descripcion_tablero = @descripTablero WHERE id_tablero = @idRecibe;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idPropietario", modificarTablero.IdUsuarioPropietarioM));
                    command.Parameters.Add(new SQLiteParameter("@nombreTablero", modificarTablero.NombreDeTableroM));
                    command.Parameters.Add(new SQLiteParameter("@descripTablero", modificarTablero.DescripcionDeTableroM));
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

        public List<Tablero> BuscarTablerosPorNombre(string nombre)
        {
            var query = "SELECT * FROM Tablero INNER JOIN Usuario ON Tablero.id_usuario_propietario = Usuario.id_usuario WHERE nombre_tablero LIKE @nombre";
            List<Tablero> listaDeTableros = new List<Tablero>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@nombre", "%" + nombre + "%"));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tablero = new Tablero
                            {
                                IdTableroM = Convert.ToInt32(reader["id_tablero"]),
                                NombreDeTableroM = reader["nombre_tablero"].ToString()!,
                                DescripcionDeTableroM = reader["descripcion_tablero"].ToString(),
                                IdUsuarioPropietarioM = Convert.ToInt32(reader["id_usuario_propietario"]),
                                NombreDePropietarioM = reader["nombre_de_usuario"].ToString()!
                            };
                            listaDeTableros.Add(tablero);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al buscar tableros por nombre en la base de datos");
                }
                finally
                {
                    connection.Close();
                }
            }
            return listaDeTableros;
        }
    }
}