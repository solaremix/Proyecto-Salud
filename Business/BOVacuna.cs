using Interface.Dto;
using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using Npgsql;
using System.Collections.Generic;

namespace Business
{
    public class BOVacuna
    {
        private readonly DOProyectoSalud _dataAccess;

        public BOVacuna()
        {
            _dataAccess = new DOProyectoSalud();
        }

        public AgregarVacunaResponseDto AgregarVacuna(AgregarVacunaRequestDto request)
        {
            var response = new AgregarVacunaResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int vacunaId = _dataAccess.InsertarVacuna(request.Vacuna, connection, transaction);
                        response.VacunaId = vacunaId; // Asegúrate de que esta propiedad exista
                        foreach (var esquema in request.EsquemaVacunacion)
                        {
                            esquema.VacunaId = vacunaId;
                            _dataAccess.InsertarEsquemaVacunacion(esquema, connection, transaction);
                        }
                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Vacuna y esquema de vacunación agregados exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.Success = false;
                        response.Message = "Error al agregar vacuna y esquema: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public ActualizarVacunaResponseDto ActualizarVacuna(ActualizarVacunaRequestDto request)
        {
            var response = new ActualizarVacunaResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.ActualizarVacuna(request.Vacuna, connection, transaction);
                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Vacuna actualizada exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.Success = false;
                        response.Message = "Error al actualizar vacuna: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public EliminarVacunaResponseDto EliminarVacuna(EliminarVacunaRequestDto request)
        {
            var response = new EliminarVacunaResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.EliminarVacuna(request.VacunaId, connection, transaction);
                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Vacuna eliminada exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.Success = false;
                        response.Message = "Error al eliminar vacuna: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public AgregarEsquemaVacunacionResponseDto AgregarEsquemaVacunacion(AgregarEsquemaVacunacionRequestDto request)
        {
            var response = new AgregarEsquemaVacunacionResponseDto();

            if (request == null || request.EsquemaVacunacion == null)
            {
                response.Success = false;
                response.Message = "El request o el esquema de vacunación es nulo.";
                return response;
            }

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.InsertarEsquemaVacunacion(request.EsquemaVacunacion, connection, transaction);
                        transaction.Commit();
                        response.Success = true;
                        response.Message = "Esquema de vacunación agregado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Error al agregar esquema de vacunación: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public ActualizarEsquemaVacunacionResponseDto ActualizarEsquemaVacunacion(ActualizarEsquemaVacunacionRequestDto request)
        {
            var response = new ActualizarEsquemaVacunacionResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.ActualizarEsquemaVacunacion(request.EsquemaVacunacion, connection, transaction);
                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Esquema de vacunación actualizado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.Success = false;
                        response.Message = "Error al actualizar esquema de vacunación: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public EliminarEsquemaVacunacionResponseDto EliminarEsquemaVacunacion(EliminarEsquemaVacunacionRequestDto request)
        {
            var response = new EliminarEsquemaVacunacionResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.EliminarEsquemaVacunacion(request.EsquemaVacunacionId, connection, transaction);
                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Esquema de vacunación eliminado exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.Success = false;
                        response.Message = "Error al eliminar esquema de vacunación: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public List<VacunaDto> ObtenerVacunasPorPerfilPaciente(int perfilPacienteId)
        {
            return _dataAccess.ObtenerVacunasPorPerfilPaciente(perfilPacienteId);
        }


    }
}
