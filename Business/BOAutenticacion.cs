using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using Npgsql;

namespace Business
{
    public class BOAutenticacion
    {
        private readonly DOProyectoSalud _dataAccess;

        public BOAutenticacion()
        {
            _dataAccess = new DOProyectoSalud();
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
