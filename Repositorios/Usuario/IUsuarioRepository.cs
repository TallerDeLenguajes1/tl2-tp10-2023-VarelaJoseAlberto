using System;
using tl2_tp10_2023_VarelaJoseAlberto.Models;

namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public interface IUsuarioRepository
    {
        void CrearUsuario(Usuario usuario);
        void ModificarUsuario(int idRecibe, Usuario usuario);
        List<Usuario> TraerTodosUsuarios();
        Usuario TraerUsuarioPorId(int id);
        void EliminarUsuarioPorId(int id);
        Usuario ObtenerUsuarioPorCredenciales(string nombreUsuario, string contrasenia);
    }
}