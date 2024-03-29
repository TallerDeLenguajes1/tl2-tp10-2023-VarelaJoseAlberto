using System.Data.SQLite;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
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
                    command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nuevoUsuario.NombreDeUsuarioM));
                    command.Parameters.Add(new SQLiteParameter("@contrasenia", nuevoUsuario.ContraseniaM));
                    command.Parameters.Add(new SQLiteParameter("@rol", (int)nuevoUsuario.RolM)); // Almacenar el valor del enum como entero
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
                            var user = new Usuario
                            {
                                IdUsuarioM = Convert.ToInt32(reader["id_usuario"]),
                                NombreDeUsuarioM = reader["nombre_de_usuario"].ToString()!,
                                ContraseniaM = reader["contrasenia"].ToString()!,
                                RolM = (Rol)Convert.ToInt32(reader["rol"])
                            };
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
                            usuario.IdUsuarioM = Convert.ToInt32(reader["id_usuario"]);
                            usuario.NombreDeUsuarioM = reader["nombre_de_usuario"].ToString()!;
                            usuario.ContraseniaM = reader["contrasenia"].ToString()!;
                            usuario.RolM = (Rol)Convert.ToInt32(reader["rol"]); // Convertir el entero almacenado en el enum
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
                    command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nuevoUsuario.NombreDeUsuarioM));
                    command.Parameters.Add(new SQLiteParameter("@contrasenia", nuevoUsuario.ContraseniaM));
                    command.Parameters.Add(new SQLiteParameter("@rol", (int)nuevoUsuario.RolM)); // Almacenar el valor del enum como entero
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
            string query = "SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreUsuario AND contrasenia = @contrasenia";
            Usuario? usuarioEncontrado = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("@nombreUsuario", nombreUsuario));
                        command.Parameters.Add(new SQLiteParameter("@contrasenia", contrasenia));

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuarioEncontrado = new Usuario
                                {
                                    IdUsuarioM = Convert.ToInt32(reader["id_usuario"]),
                                    NombreDeUsuarioM = reader["nombre_de_usuario"].ToString()!,
                                    ContraseniaM = reader["contrasenia"].ToString()!,
                                    RolM = (Rol)Convert.ToInt32(reader["rol"])
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
            // if (usuarioEncontrado == null)
            // {
            //     throw new Exception("Usuario no Existe");
            // }
            return usuarioEncontrado!;
        }

        public List<Usuario> BuscarUsuarioPorNombre(string nombre)
        {
            var query = "SELECT * FROM Usuario WHERE nombre_de_usuario LIKE @nombre";
            List<Usuario> listaDeUsuarios = new List<Usuario>();
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
                            var usuario = new Usuario
                            {
                                IdUsuarioM = Convert.ToInt32(reader["id_usuario"]),
                                NombreDeUsuarioM = reader["nombre_de_usuario"].ToString()!,
                                ContraseniaM = reader["contrasenia"].ToString()!,
                                RolM = (Rol)Convert.ToInt32(reader["rol"])
                            };
                            listaDeUsuarios.Add(usuario);
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al buscar usuarios por nombre en la base de datos");
                }
                finally
                {
                    connection.Close();
                }
            }
            return listaDeUsuarios;
        }

        public bool ExisteUsuario(string nombreDeUsuario)
        {
            bool existe = false;
            var query = "SELECT * FROM usuario WHERE nombre_de_usuario = @nombre;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var command = new SQLiteCommand(query, connection);
                    command.Parameters.Add(new SQLiteParameter("@nombre", nombreDeUsuario));
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            existe = true;
                        }
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Hubo un problema al buscar el nombre del usuario en la base de datos");
                }
                finally
                {
                    connection.Close();
                }
            }
            return existe;
        }
    }
}