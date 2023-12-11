using System;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public interface IUsuarioRepository
    {
        public void CrearUsuario(Usuario usuario);
        public void ModificarUsuario(int idRecibe, Usuario usuario);
        public List<Usuario> TraerTodosUsuarios();
        public Usuario TraerUsuarioPorId(int id);
        public void EliminarUsuarioPorId(int id);
        public Usuario ObtenerUsuarioPorCredenciales(string nombreUsuario, string contrasenia);
    }
}