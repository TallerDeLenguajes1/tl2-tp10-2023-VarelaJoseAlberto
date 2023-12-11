using System.Data.SQLite;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        // private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";
        private readonly string connectionString;

        public UsuarioRepository(string CadenaConexion)
        {
            connectionString = CadenaConexion;
        }


        public void CrearUsuario(Usuario nuevoUsuario)
        {
            var query = "INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES (@nombre_de_usuario, @contrasenia, @rol)";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nuevoUsuario.NombreDeUsuario));
                    command.Parameters.Add(new SQLiteParameter("@contrasenia", nuevoUsuario.Contrasenia));
                    command.Parameters.Add(new SQLiteParameter("@rol", (int)nuevoUsuario.Rol)); // Almacenar el valor del enum como entero
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema en la BD al crear un nuevo usuario.");
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public List<Usuario> TraerTodosUsuarios()
        {
            var query = "SELECT * FROM usuario;";
            List<Usuario> listaDeUsuarios = new List<Usuario>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new Usuario();
                            user.IdUsuario = Convert.ToInt32(reader["id_usuario"]);
                            user.NombreDeUsuario = reader["nombre_de_usuario"].ToString()!;
                            user.Contrasenia = reader["contrasenia"].ToString()!;
                            user.Rol = (Rol)Convert.ToInt32(reader["rol"]); // Convertir el entero almacenado en el enum
                            listaDeUsuarios.Add(user);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD Los usuarios");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (listaDeUsuarios == null)
            {
                throw new Exception("Lista de Usuarios vacia");
            }
            return listaDeUsuarios;
        }


        public Usuario TraerUsuarioPorId(int idRecibe)
        {
            var usuario = new Usuario();
            var query = "SELECT * FROM Usuario WHERE id_usuario = @id_usuario";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@id_usuario", idRecibe));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuario.IdUsuario = Convert.ToInt32(reader["id_usuario"]);
                            usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString()!;
                            usuario.Contrasenia = reader["contrasenia"].ToString()!;
                            usuario.Rol = (Rol)Convert.ToInt32(reader["rol"]); // Convertir el entero almacenado en el enum
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD al usuario");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (usuario == null)
            {
                throw new Exception("Usuario no Existe");
            }
            return usuario;
        }


        public void EliminarUsuarioPorId(int idRecibe)
        {
            var query = "DELETE FROM Usuario WHERE id_usuario = @id_usuario";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@id_usuario", idRecibe));
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al borrar al usuario");
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public void ModificarUsuario(int idRecibe, Usuario nuevoUsuario)
        {
            var query = "UPDATE Usuario SET nombre_de_usuario = @nombre_de_usuario, contrasenia = @contrasenia, rol = @rol WHERE id_usuario = @id_usuario;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nuevoUsuario.NombreDeUsuario));
                    command.Parameters.Add(new SQLiteParameter("@contrasenia", nuevoUsuario.Contrasenia));
                    command.Parameters.Add(new SQLiteParameter("@rol", (int)nuevoUsuario.Rol)); // Almacenar el valor del enum como entero
                    command.Parameters.Add(new SQLiteParameter("@id_usuario", idRecibe));
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al Modificar al usuario");
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public Usuario ObtenerUsuarioPorCredenciales(string nombreUsuario, string contrasenia)
        {
            Usuario usuarioEncontrado = new Usuario();
            string query = "SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreUsuario AND contrasenia = @contrasenia";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                        command.Parameters.AddWithValue("@contrasenia", contrasenia);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuarioEncontrado = new Usuario
                                {
                                    IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                                    NombreDeUsuario = reader["nombre_de_usuario"].ToString()!,
                                    Contrasenia = reader["contrasenia"].ToString()!,
                                    Rol = (Rol)Convert.ToInt32(reader["rol"])
                                };
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al extraer desde la BD las credenciales del usuario");
                }
                finally
                {
                    connection.Close();
                }
            }
            if (usuarioEncontrado == null)
            {
                throw new Exception("Usuario no Existe");
            }
            return usuarioEncontrado;
        }
    }
}