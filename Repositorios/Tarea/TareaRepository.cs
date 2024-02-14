using System.Data.SQLite;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public class TareaRepository : ITareaRepository
    {
        // private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";
        private readonly string connectionString;

        public TareaRepository(string CadenaConexion)
        {
            connectionString = CadenaConexion;
        }

        public void CrearTarea(int idTablero, Tarea nuevaTarea)
        {
            var query = "INSERT INTO Tarea (id_tablero, nombre_tarea, estado_tarea, descripcion_tarea, color_tarea, id_usuario_asignado) " +
                        "VALUES (@idTablero, @nombreTarea, @estadoTarea, @descripcionTarea, @colorTarea, @idUsuarioAsignado);";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                    command.Parameters.Add(new SQLiteParameter("@nombreTarea", nuevaTarea.NombreTareaM));
                    command.Parameters.Add(new SQLiteParameter("@estadoTarea", nuevaTarea.EstadoTareaM.ToString()));
                    command.Parameters.Add(new SQLiteParameter("@descripcionTarea", nuevaTarea.DescripcionTareaM));
                    command.Parameters.Add(new SQLiteParameter("@colorTarea", nuevaTarea.ColorM));
                    command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", nuevaTarea.IdUsuarioAsignadoM));
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema en la BD al crear un nueva Tarea.");
                }
                finally
                {
                    connection.Close();
                }

            }
        }


        public List<Tarea> ListarTodasLasTareas()
        {
            var query = "SELECT * FROM Tarea";
            List<Tarea> listaDeTareas = new List<Tarea>();
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
                            var tarea = new Tarea
                            {
                                IdTareaM = Convert.ToInt32(reader["id_tarea"]),
                                IdTableroM = Convert.ToInt32(reader["id_tablero"]),
                                NombreTareaM = reader["nombre_tarea"].ToString()!,
                                DescripcionTareaM = reader["descripcion_tarea"].ToString(),
                                ColorM = reader["color_tarea"].ToString(),
                                EstadoTareaM = (EstadoTarea)Enum.Parse(typeof(EstadoTarea), reader["estado_tarea"].ToString()!)
                            };
                            // Verificar si IdUsuarioAsignado es nulo en la base de datos
                            if (reader.IsDBNull(reader.GetOrdinal("id_usuario_asignado")))
                            {
                                tarea.IdUsuarioAsignadoM = null; // Asignar null si es nulo en la base de datos
                            }
                            else
                            {
                                tarea.IdUsuarioAsignadoM = Convert.ToInt32(reader["id_usuario_asignado"]);
                            }
                            listaDeTareas.Add(tarea);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD la lista de tareas");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (listaDeTareas == null)
            {
                throw new Exception("Lista de Tarea vacia");
            }
            return listaDeTareas;
        }


        public List<Tarea> ListarTareasDeUsuario(int idUsuario)
        {
            var query = "SELECT * FROM Tarea WHERE id_usuario_asignado = @id_usuario";
            List<Tarea> listaDeTareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@id_usuario", idUsuario));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea
                            {
                                IdTareaM = Convert.ToInt32(reader["id_tarea"]),
                                IdTableroM = Convert.ToInt32(reader["id_tablero"]),
                                NombreTareaM = reader["nombre_tarea"].ToString()!,
                                DescripcionTareaM = reader["descripcion_tarea"].ToString(),
                                ColorM = reader["color_tarea"].ToString(),
                                EstadoTareaM = (EstadoTarea)Enum.Parse(typeof(EstadoTarea), reader["estado_tarea"].ToString()!),
                                IdUsuarioAsignadoM = Convert.ToInt32(reader["id_usuario_asignado"])
                            };
                            listaDeTareas.Add(tarea);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD la lista de tareas");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (listaDeTareas == null)
            {
                throw new Exception("Lista de Tarea vacia");
            }
            return listaDeTareas;
        }


        public List<Tarea> ListarTareasDeTablero(int idRecibe)
        {
            var query = "SELECT * FROM Tarea WHERE id_tablero = @idRecibe";
            List<Tarea> listaDeTareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idRecibe", idRecibe));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea
                            {
                                IdTareaM = Convert.ToInt32(reader["id_tarea"]),
                                IdTableroM = Convert.ToInt32(reader["id_tablero"]),
                                NombreTareaM = reader["nombre_tarea"].ToString()!,
                                DescripcionTareaM = reader["descripcion_tarea"].ToString(),
                                ColorM = reader["color_tarea"].ToString(),
                                EstadoTareaM = (EstadoTarea)Enum.Parse(typeof(EstadoTarea), reader["estado_tarea"].ToString()!),
                                IdUsuarioAsignadoM = Convert.ToInt32(reader["id_usuario_asignado"])
                            };
                            listaDeTareas.Add(tarea);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD la lista de tareas");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (listaDeTareas == null)
            {
                throw new Exception("Lista de Tarea vacia");
            }
            return listaDeTareas;
        }


        public Tarea ObtenerTareaPorId(int idTarea)
        {
            var query = "SELECT * FROM Tarea WHERE id_tarea = @idTarea;";
            var tarea = new Tarea();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tarea.IdTareaM = Convert.ToInt32(reader["id_tarea"]);
                            tarea.IdTableroM = Convert.ToInt32(reader["id_tablero"]);
                            tarea.NombreTareaM = reader["nombre_tarea"].ToString()!;
                            tarea.DescripcionTareaM = reader["descripcion_tarea"].ToString();
                            tarea.ColorM = reader["color_tarea"].ToString();
                            tarea.EstadoTareaM = (EstadoTarea)Enum.Parse(typeof(EstadoTarea), reader["estado_tarea"].ToString()!);
                            // Verificar si IdUsuarioAsignado es nulo en la base de datos
                            if (reader.IsDBNull(reader.GetOrdinal("id_usuario_asignado")))
                            {
                                tarea.IdUsuarioAsignadoM = null; // Asignar null si es nulo en la base de datos
                            }
                            else
                            {
                                tarea.IdUsuarioAsignadoM = Convert.ToInt32(reader["id_usuario_asignado"]);
                            }

                            return tarea;
                        }
                    }

                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD la tarea");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (tarea == null)
            {
                throw new Exception("Tarea no Existe");
            }
            return tarea;
        }


        public void EliminarTarea(int idTarea)
        {
            var query = "DELETE FROM Tarea WHERE id_tarea = @idTarea;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al borrar la tarea");

                }
                finally
                {
                    connection.Close(); // Asegúrate de cerrar la conexión en el bloque finally
                }
            }
        }


        public void ModificarTarea(int idTarea, Tarea tareaModificada)
        {
            var query = "UPDATE Tarea " +
                        "SET id_tablero = @idTablero, nombre_tarea = @nombreTarea, descripcion_tarea = @descripcionTarea, estado_tarea = @estadoTarea, color_tarea = @colorTarea, id_usuario_asignado = @idUsuarioAsignado " +
                        "WHERE id_tarea = @idTarea;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@idTablero", tareaModificada.IdTableroM));
                    command.Parameters.Add(new SQLiteParameter("@nombreTarea", tareaModificada.NombreTareaM));
                    command.Parameters.Add(new SQLiteParameter("@descripcionTarea", tareaModificada.DescripcionTareaM));
                    command.Parameters.Add(new SQLiteParameter("@estadoTarea", tareaModificada.EstadoTareaM.ToString()));
                    command.Parameters.Add(new SQLiteParameter("@colorTarea", tareaModificada.ColorM));
                    command.Parameters.Add(new SQLiteParameter("@idUsuarioAsignado", tareaModificada.IdUsuarioAsignadoM));
                    command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al Modificar la tarea");
                }
                finally
                {
                    connection.Close();
                }

            }
        }


        public void AsignarUsuarioATarea(int idUsuario, int idTarea)
        {
            var query = "UPDATE Tarea SET id_usuario_asignado = @idUsuario WHERE id_tarea = @idTarea";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        command.Parameters.AddWithValue("@idTarea", idTarea);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al asignar un usuario");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }

}
