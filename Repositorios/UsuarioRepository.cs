using System.Data.SQLite;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

        public void CrearUsuario(Usuario nuevoUsuario)
        {
            var query = "INSERT INTO Usuario (nombre_de_usuario, contrasenia, rol) VALUES (@nombre_de_usuario, @contrasenia, @rol)";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nuevoUsuario.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", nuevoUsuario.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@rol", (int)nuevoUsuario.Rol)); // Almacenar el valor del enum como entero
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<Usuario> TraerTodosUsuarios()
        {
            var query = "SELECT * FROM usuario;";
            List<Usuario> listaDeUsuarios = new List<Usuario>();
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new Usuario();
                        user.IdUsuario = Convert.ToInt32(reader["id_usuario"]);
                        user.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        user.Contrasenia = reader["contrasenia"].ToString();
                        user.Rol = (Rol)Convert.ToInt32(reader["rol"]); // Convertir el entero almacenado en el enum
                        listaDeUsuarios.Add(user);
                    }
                }
                connection.Close();
            }
            return listaDeUsuarios;
        }

        public Usuario TraerUsuarioPorId(int idRecibe)
        {
            var query = "SELECT * FROM Usuario WHERE id_usuario = @id_usuario";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id_usuario", idRecibe));

                var usuario = new Usuario();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.IdUsuario = Convert.ToInt32(reader["id_usuario"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuario.Rol = (Rol)Convert.ToInt32(reader["rol"]); // Convertir el entero almacenado en el enum
                    }
                    connection.Close();
                    return usuario;
                }
            }
        }

        public void EliminarUsuarioPorId(int idRecibe)
        {
            var query = "DELETE FROM Usuario WHERE id_usuario = @id_usuario";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@id_usuario", idRecibe));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ModificarUsuario(int idRecibe, Usuario nuevoUsuario)
        {
            var query = "UPDATE Usuario SET nombre_de_usuario = @nombre_de_usuario, contrasenia = @contrasenia, rol = @rol WHERE id_usuario = @id_usuario;";
            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nuevoUsuario.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@contrasenia", nuevoUsuario.Contrasenia));
                command.Parameters.Add(new SQLiteParameter("@rol", (int)nuevoUsuario.Rol)); // Almacenar el valor del enum como entero
                command.Parameters.Add(new SQLiteParameter("@id_usuario", idRecibe));
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public Usuario ObtenerUsuarioPorCredenciales(string nombreUsuario, string contrasenia)
        {
            Usuario? usuarioEncontrado = null;
            string query = "SELECT * FROM Usuario WHERE nombre_de_usuario = @nombreUsuario AND contrasenia = @contrasenia";

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
                                NombreDeUsuario = reader["nombre_de_usuario"].ToString(),
                                Contrasenia = reader["contrasenia"].ToString(),
                                Rol = (Rol)Convert.ToInt32(reader["rol"])
                            };
                        }
                    }
                }

                connection.Close();
            }
            return usuarioEncontrado;
        }
    }
}