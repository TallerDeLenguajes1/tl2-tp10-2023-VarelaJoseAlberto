using System.Collections.Generic;
using tl2_tp10_2023_VarelaJoseAlberto.Models;
using tl2_tp10_2023_VarelaJoseAlberto.ViewModels;

namespace tl2_tp10_2023_VarelaJoseAlberto.ViewModels
{
    public class ListarUsuariosViewModel
    {
        public List<UsuarioViewModel>? UsuariosVM { get; set; }

        public ListarUsuariosViewModel(List<UsuarioViewModel> usuariosVM)
        {
            UsuariosVM = usuariosVM;
        }


        public ListarUsuariosViewModel()
        {
            UsuariosVM = new List<UsuarioViewModel>();
        }

    }
}
