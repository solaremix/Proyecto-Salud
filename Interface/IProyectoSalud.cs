using Interface.Dto.Request;
using Interface.Dto.Response;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Interface
{
    [ServiceContract]
    public interface IProyectoSalud
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "RegistrarUsuario", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RegistrarUsuarioResponseDto RegistrarUsuario(RegistrarUsuarioRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "IniciarSesion", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IniciarSesionResponseDto IniciarSesion(IniciarSesionRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AgregarPerfilPaciente", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgregarPerfilPacienteResponseDto AgregarPerfilPaciente(AgregarPerfilPacienteRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ModificarDatosUsuario", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ModificarDatosUsuarioResponseDto ModificarDatosUsuario(ModificarDatosUsuarioRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "CambiarContrasena", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CambiarContrasenaResponseDto CambiarContrasena(CambiarContrasenaRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AgregarRegistroMedico", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AgregarRegistroMedicoResponseDto AgregarRegistroMedico(AgregarRegistroMedicoRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ModificarDatosPaciente", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ModificarDatosPacienteResponseDto ModificarDatosPaciente(ModificarDatosPacienteRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "EliminarPerfilPaciente", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EliminarPerfilPacienteResponseDto EliminarPerfilPaciente(EliminarPerfilPacienteRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "ModificarRegistroMedico", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ModificarRegistroMedicoResponseDto ModificarRegistroMedico(ModificarRegistroMedicoRequestDto request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "EliminarRegistroMedico", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EliminarRegistroMedicoResponseDto EliminarRegistroMedico(EliminarRegistroMedicoRequestDto request);
    }
}
