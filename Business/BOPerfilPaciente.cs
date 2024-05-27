using Interface.Dto;
using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using Npgsql;

namespace Business
{
    public class BOPerfilPaciente
    {
        private readonly DOProyectoSalud _dataAccess;

        public BOPerfilPaciente()
        {
            _dataAccess = new DOProyectoSalud();
        }

        public ModificarDatosPacienteResponseDto ModificarDatosPaciente(ModificarDatosPacienteRequestDto request)
        {
            var response = new ModificarDatosPacienteResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.ActualizarPerfilPaciente(request.perfilPacienteId, request.perfilPaciente, connection, transaction);

                        transaction.Commit();

                        response.success = true;
                        response.message = "Datos del paciente modificados exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.success = false;
                        response.message = "No se pudo modificar los datos del paciente: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public EliminarPerfilPacienteResponseDto EliminarPerfilPaciente(EliminarPerfilPacienteRequestDto request)
        {
            var response = new EliminarPerfilPacienteResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.EliminarPerfilPaciente(request.perfilPacienteId, connection, transaction);

                        transaction.Commit();

                        response.success = true;
                        response.message = "Perfil del paciente eliminado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.success = false;
                        response.message = "No se pudo eliminar el perfil del paciente: " + ex.Message;
                    }
                }
            }

            return response;
        }
    }
}
