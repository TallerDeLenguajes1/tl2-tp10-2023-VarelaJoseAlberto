namespace tl2_tp10_2023_VarelaJoseAlberto.Repositorios
{
    public static class Autorizacion
    {
        public static bool EstaAutentificado(HttpContext context)
        {
            return context.Session.GetInt32("IdUsuario").HasValue;
        }

        public static bool EsAdmin(HttpContext context)
        {
            return context.Session.GetString("Rol") == "admin";
        }
        public static string ObtenerRol(HttpContext context)
        {
            return context.Session.GetString("Rol")!;
        }

        public static int ObtenerIdUsuario(HttpContext context)
        {
            return (int)context.Session.GetInt32("IdUsuario")!;
        }
    }
}