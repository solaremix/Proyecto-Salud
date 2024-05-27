using Interface.Dto;
using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using Npgsql;

namespace Business
{
    public class BORegistroMedico
    {
        private readonly DOProyectoSalud _dataAccess;

        public BORegistroMedico()
        {
            _dataAccess = new DOProyectoSalud();
        }

        public ModificarRegistroMedicoResponseDto ModificarRegistroMedico(ModificarRegistroMedicoRequestDto request)
        {
            var response = new ModificarRegistroMedicoResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.ActualizarRegistroMedico(request.registroMedicoId, request.registroMedico, connection, transaction);

                        _dataAccess.ActualizarDatosMedicos(request.registroMedicoId, request.registroMedico.datos, connection, transaction);

                        transaction.Commit();

                        response.success = true;
                        response.message = "Registro médico modificado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.success = false;
                        response.message = "No se pudo modificar el registro médico: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public EliminarRegistroMedicoResponseDto EliminarRegistroMedico(EliminarRegistroMedicoRequestDto request)
        {
            var response = new EliminarRegistroMedicoResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.EliminarRegistroMedico(request.registroMedicoId, connection, transaction);

                        transaction.Commit();

                        response.success = true;
                        response.message = "Registro médico eliminado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.success = false;
                        response.message = "No se pudo eliminar el registro médico: " + ex.Message;
                    }
                }
            }

            return response;
        }
    }
}
