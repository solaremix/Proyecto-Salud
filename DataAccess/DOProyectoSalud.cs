using Interface.Dto;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DOProyectoSalud
    {
        private readonly string _connectionString;

        public DOProyectoSalud()
        {
            _connectionString = "Host=localhost;Port=5432;Database=ProyectoSalud;Username=postgres;Password=Starbucks2020";
        }

        public int InsertarPadre(PadreDto padre)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Padres (nombre, apellido, dni, genero, fechaNacimiento) VALUES (@nombre, @apellido, @dni, @genero, @fechaNacimiento) RETURNING id";
                    command.Parameters.AddWithValue("@nombre", padre.nombre);
                    command.Parameters.AddWithValue("@apellido", padre.apellido);
                    command.Parameters.AddWithValue("@dni", padre.dni);
                    command.Parameters.AddWithValue("@genero", padre.genero);
                    command.Parameters.AddWithValue("@fechaNacimiento", padre.fechaNacimiento);

                    return (int)command.ExecuteScalar();
                }
            }
        }

        public void InsertarUsuario(UsuarioDto usuario)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Usuarios (email, contraseña, padreId) VALUES (@Email, @Contrasena, @PadreId)";
                    command.Parameters.AddWithValue("@Email", usuario.email);
                    command.Parameters.AddWithValue("@Contrasena", usuario.contrasena);
                    command.Parameters.AddWithValue("@PadreId", usuario.padre.id); // Asegurarse de que esto está correctamente establecido

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
