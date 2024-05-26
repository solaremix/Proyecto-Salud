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

        public IniciarSesionResponseDto IniciarSesion(IniciarSesionRequestDto request)
        {
            var response = new IniciarSesionResponseDto();

            using (var connection = new NpgsqlConnection(_dataAccess.ConnectionString))
            {
                connection.Open();
                try
                {
                    var usuario = _dataAccess.ObtenerUsuarioPorEmailYContrasena(request.email, request.contrasena, connection);
                    if (usuario != null)
                    {
                        response.Success = true;
                        response.Message = "Inicio de sesión exitoso.";
                        response.Usuario = usuario;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Email o contraseña incorrectos.";
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = "Error al iniciar sesión: " + ex.Message;
                }
            }

            return response;
        }
    }
}




