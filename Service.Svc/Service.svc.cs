using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using Interface;
using Business;
using Interface.Dto;
using System.ServiceModel.Web;

namespace Service.Svc
{
    public class Service : IProyectoSalud
    {
        private readonly BOUsuario _boUsuario;

        public Service()
        {
            _boUsuario = new BOUsuario();
        }

        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "RegistrarUsuario")]
        public void RegistrarUsuario(UsuarioDto usuario)
        {
            _boUsuario.RegistrarUsuario(usuario);
        }
    }
}
