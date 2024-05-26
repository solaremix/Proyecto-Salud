using System.ServiceModel;
using System.ServiceModel.Web;
using Interface.Dto.Request;
using Interface.Dto.Response;

namespace Interface
{
    [ServiceContract]
    public interface IProyectoSalud
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "RegistrarUsuario")]
        RegistrarUsuarioResponseDto RegistrarUsuario(RegistrarUsuarioRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "IniciarSesion")]
        IniciarSesionResponseDto IniciarSesion(IniciarSesionRequestDto request);
    }
}
