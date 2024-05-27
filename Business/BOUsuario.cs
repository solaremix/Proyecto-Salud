using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using Npgsql;
using Interface.Dto;

namespace Business
{
    public class BOUsuario
    {
        private readonly DOProyectoSalud _dataAccess;

        public BOUsuario()
        {
            _dataAccess = new DOProyectoSalud();
        }

        public RegistrarUsuarioResponseDto RegistrarUsuario(RegistrarUsuarioRequestDto request)
        {
            var response = new RegistrarUsuarioResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int padreId = _dataAccess.InsertarPadre(request.usuario.padre, connection, transaction);
                        request.usuario.padre.id = padreId;

                        _dataAccess.InsertarUsuario(request.usuario, connection, transaction);

                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Su registro ha sido exitoso.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.Success = false;
                        response.Message = "No se pudo registrar al usuario: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public AgregarPerfilPacienteResponseDto AgregarPerfilPaciente(int usuarioId, PerfilPacienteDto perfilPaciente)
        {
            var response = new AgregarPerfilPacienteResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.InsertarPerfilPaciente(usuarioId, perfilPaciente, connection, transaction);

                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Perfil de paciente agregado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.Success = false;
                        response.Message = "No se pudo agregar el perfil de paciente: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public AgregarRegistroMedicoResponseDto AgregarRegistroMedico(AgregarRegistroMedicoRequestDto request)
        {
            var response = new AgregarRegistroMedicoResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int registroMedicoId = _dataAccess.InsertarRegistroMedico(request.perfilPacienteId, request.registroMedico, connection, transaction);

                        _dataAccess.InsertarDatosMedicos(registroMedicoId, request.registroMedico.datos, connection, transaction);

                        transaction.Commit();

                        response.success = true;
                        response.message = "Registro médico agregado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.success = false;
                        response.message = "No se pudo agregar el registro médico: " + ex.Message;
                    }
                }
            }

            return response;
        }
    }
}
