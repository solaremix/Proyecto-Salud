using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using Npgsql;

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
    }
}




