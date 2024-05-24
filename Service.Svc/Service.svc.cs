using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using Interface;
using Business;
using Interface.Dto;
using System.ServiceModel.Web;
using Interface.Dto.Request;
using Interface.Dto.Response;
using System.ServiceModel.Description;
using System.ServiceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public RegistrarUsuarioResponseDto RegistrarUsuario(RegistrarUsuarioRequestDto request)
        {
            return _boUsuario.RegistrarUsuario(request);
        }
    }
}




