using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Interface.Dto;
using Interface.Dto.Request;
using Interface.Dto.Response;
using Npgsql;

namespace Business
{
    public class BONotificacion
    {
        private readonly DOProyectoSalud _dataAccess;

        public BONotificacion()
        {
            _dataAccess = new DOProyectoSalud();
        }

        public AgregarNotificacionResponseDto AgregarNotificacion(AgregarNotificacionRequestDto request)
        {
            var response = new AgregarNotificacionResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var notificacion = new NotificacionDto
                        {
                            UsuarioId = request.UsuarioId,
                            Descripcion = request.Descripcion,
                            FechaHora = request.FechaHora
                        };

                        // Insertar notificación
                        _dataAccess.InsertarNotificacion(notificacion, connection, transaction);

                        // Commit de la transacción
                        transaction.Commit();

                        response.Success = true;
                        response.Message = "Notificación agregada exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.Success = false;
                        response.Message = "Error al agregar notificación: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public List<NotificacionDto> ObtenerNotificacionesPorUsuario(int usuarioId)
        {
            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                return _dataAccess.ObtenerNotificacionesPorUsuario(usuarioId, connection);
            }
        }
    }
}
