using System.ServiceModel;
using System.ServiceModel.Web;
using Interface.Dto;

namespace Interface
{
    [ServiceContract]
    public interface IProyectoSalud
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "RegistrarUsuario")]
        void RegistrarUsuario(UsuarioDto usuario);
    }
}
