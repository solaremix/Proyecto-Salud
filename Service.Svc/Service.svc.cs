using System.ServiceModel;
using System.ServiceModel.Web;
using Interface;
using Interface.Dto.Request;
using Interface.Dto.Response;
using Business;

namespace Service.Svc
{
    public class Service : IProyectoSalud
    {
        private readonly BOUsuario _boUsuario;

        public Service()
        {
            _boUsuario = new BOUsuario();
        }

        public RegistrarUsuarioResponseDto RegistrarUsuario(RegistrarUsuarioRequestDto request)
        {
            return _boUsuario.RegistrarUsuario(request);
        }

        public IniciarSesionResponseDto IniciarSesion(IniciarSesionRequestDto request)
        {
            return _boUsuario.IniciarSesion(request);
        }
    }
}
