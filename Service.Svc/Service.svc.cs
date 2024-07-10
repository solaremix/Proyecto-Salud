using System.ServiceModel;
using System.ServiceModel.Web;
using Interface;
using Interface.Dto.Request;
using Interface.Dto.Response;
using Business;
using System;
using Interface.Dto;
using System.Collections.Generic;

namespace Service.Svc
{
    public class Service : IProyectoSalud
    {
        private readonly BOUsuario _boUsuario;
        private readonly BOAutenticacion _boAutenticacion;
        private readonly BOPerfilUsuario _boPerfilUsuario;
        private readonly BOPerfilPaciente _boPerfilPaciente;
        private readonly BORegistroMedico _boRegistroMedico;
        private readonly BOVacuna _boVacuna;

        public Service()
        {
            _boUsuario = new BOUsuario();
            _boAutenticacion = new BOAutenticacion();
            _boPerfilUsuario = new BOPerfilUsuario();
            _boPerfilPaciente = new BOPerfilPaciente();
            _boRegistroMedico = new BORegistroMedico();
            _boVacuna = new BOVacuna();
        }

        public RegistrarUsuarioResponseDto RegistrarUsuario(RegistrarUsuarioRequestDto request)
        {
            return _boUsuario.RegistrarUsuario(request);
        }

        public IniciarSesionResponseDto IniciarSesion(IniciarSesionRequestDto request)
        {
            return _boAutenticacion.IniciarSesion(request);
        }

        public AgregarPerfilPacienteResponseDto AgregarPerfilPaciente(AgregarPerfilPacienteRequestDto request)
        {
            return _boUsuario.AgregarPerfilPaciente(request.UsuarioId, request.PerfilPaciente);
        }

        public ModificarDatosUsuarioResponseDto ModificarDatosUsuario(ModificarDatosUsuarioRequestDto request)
        {
            return _boPerfilUsuario.ModificarDatosUsuario(request);
        }

        public CambiarContrasenaResponseDto CambiarContrasena(CambiarContrasenaRequestDto request)
        {
            return _boPerfilUsuario.CambiarContrasena(request);
        }

        public AgregarRegistroMedicoResponseDto AgregarRegistroMedico(AgregarRegistroMedicoRequestDto request)
        {
            return _boUsuario.AgregarRegistroMedico(request);
        }

        public ModificarDatosPacienteResponseDto ModificarDatosPaciente(ModificarDatosPacienteRequestDto request)
        {
            return _boPerfilPaciente.ModificarDatosPaciente(request);
        }

        public EliminarPerfilPacienteResponseDto EliminarPerfilPaciente(EliminarPerfilPacienteRequestDto request)
        {
            return _boPerfilPaciente.EliminarPerfilPaciente(request);
        }

        public ModificarRegistroMedicoResponseDto ModificarRegistroMedico(ModificarRegistroMedicoRequestDto request)
        {
            return _boRegistroMedico.ModificarRegistroMedico(request);
        }

        public EliminarRegistroMedicoResponseDto EliminarRegistroMedico(EliminarRegistroMedicoRequestDto request)
        {
            return _boRegistroMedico.EliminarRegistroMedico(request);
        }

        public ListarPacientesPorUsuarioResponseDto ListarPacientesPorUsuario(ListarPacientesPorUsuarioRequestDto request)
        {
            return _boUsuario.ObtenerPerfilesPorUsuarioId(request.UsuarioId);
        }

        public ListarRegistroMedicoPorPerfilPacienteResponseDto ListarRegistroMedicoPorPerfilPaciente(ListarRegistroMedicoPorPerfilPacienteRequestDto request)
        {
            var response = new ListarRegistroMedicoPorPerfilPacienteResponseDto();

            try
            {
                Console.WriteLine($"PerfilPacienteId recibido: {request.PerfilPacienteId}"); // Debug
                var registrosMedicos = _boUsuario.ObtenerRegistrosMedicosPorPerfilPacienteId(request.PerfilPacienteId);
                response.RegistrosMedicos = registrosMedicos;
                response.Success = true;
                response.Message = "Lista de registros médicos recuperada exitosamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error al obtener la lista de registros médicos: " + ex.Message;
            }

            return response;
        }

        // Nuevos métodos agregados
        public AgregarVacunaResponseDto AgregarVacuna(AgregarVacunaRequestDto request)
        {
            return _boVacuna.AgregarVacuna(request);
        }

        public ActualizarVacunaResponseDto ActualizarVacuna(ActualizarVacunaRequestDto request)
        {
            return _boVacuna.ActualizarVacuna(request);
        }

        public EliminarVacunaResponseDto EliminarVacuna(EliminarVacunaRequestDto request)
        {
            return _boVacuna.EliminarVacuna(request);
        }

        public AgregarEsquemaVacunacionResponseDto AgregarEsquemaVacunacion(AgregarEsquemaVacunacionRequestDto request)
        {
            return _boVacuna.AgregarEsquemaVacunacion(request);
        }

        public ActualizarEsquemaVacunacionResponseDto ActualizarEsquemaVacunacion(ActualizarEsquemaVacunacionRequestDto request)
        {
            return _boVacuna.ActualizarEsquemaVacunacion(request);
        }

        public EliminarEsquemaVacunacionResponseDto EliminarEsquemaVacunacion(EliminarEsquemaVacunacionRequestDto request)
        {
            return _boVacuna.EliminarEsquemaVacunacion(request);
        }
        public List<VacunaDto> ObtenerVacunasPorPerfilPaciente(ObtenerVacunasPorPerfilPacienteRequestDto request)
        {
            return _boVacuna.ObtenerVacunasPorPerfilPaciente(request.PerfilPacienteId);
        }


    }
}
