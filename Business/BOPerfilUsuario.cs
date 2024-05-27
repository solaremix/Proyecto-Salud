using Interface.Dto;
using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using Npgsql;

namespace Business
{
    public class BOPerfilUsuario
    {
        private readonly DOProyectoSalud _dataAccess;

        public BOPerfilUsuario()
        {
            _dataAccess = new DOProyectoSalud();
        }

        public ModificarDatosUsuarioResponseDto ModificarDatosUsuario(ModificarDatosUsuarioRequestDto request)
        {
            var response = new ModificarDatosUsuarioResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Actualizar los datos del usuario
                        _dataAccess.ActualizarUsuario(request.usuarioId, request.email, request.contrasena, connection, transaction);

                        // Actualizar los datos del padre
                        _dataAccess.ActualizarPadre(request.padre, connection, transaction);

                        transaction.Commit();

                        response.success = true;
                        response.message = "Datos del usuario modificados exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.success = false;
                        response.message = "No se pudo modificar los datos del usuario: " + ex.Message;
                    }
                }
            }

            return response;
        }

        public CambiarContrasenaResponseDto CambiarContrasena(CambiarContrasenaRequestDto request)
        {
            var response = new CambiarContrasenaResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _dataAccess.ActualizarContrasena(request.email, request.nuevaContrasena, connection, transaction);

                        transaction.Commit();

                        response.success = true;
                        response.message = "Contraseña cambiada exitosamente.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        response.success = false;
                        response.message = "No se pudo cambiar la contraseña: " + ex.Message;
                    }
                }
            }

            return response;
        }
    }
}
