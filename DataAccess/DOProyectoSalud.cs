using Interface.Dto;
using Npgsql;
using System.Configuration;



namespace DataAccess
{
    public class DOProyectoSalud
    {
        private readonly string _connectionString;

        public DOProyectoSalud()
        {
            //_connectionString = "Host=localhost;Port=5432;Database=ProyectoSalud;Username=postgres;Password=Starbucks2020";
            _connectionString = ConfigurationManager.ConnectionStrings["ProyectoSalud"].ConnectionString;
        }

        public string ConnectionString => _connectionString;

        public int InsertarPadre(PadreDto padre, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;
                //command.CommandText = "SET datestyle = 'ISO, YMD';";
                command.CommandText = "INSERT INTO Padres (nombre, apellido, dni, genero, fechaNacimiento) VALUES (@nombre, @apellido, @dni, @genero, @fechaNacimiento) RETURNING id";
                command.Parameters.AddWithValue("@nombre", padre.nombre);
                command.Parameters.AddWithValue("@apellido", padre.apellido);
                command.Parameters.AddWithValue("@dni", padre.dni);
                command.Parameters.AddWithValue("@genero", padre.genero);
                command.Parameters.AddWithValue("@fechaNacimiento", padre.fechaNacimiento);

                return (int)command.ExecuteScalar();
            }
        }

        public void InsertarUsuario(UsuarioDto usuario, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO Usuarios (email, contraseña, padreId) VALUES (@email, @contrasena, @padreId)";
                command.Parameters.AddWithValue("@email", usuario.email);
                command.Parameters.AddWithValue("@contrasena", usuario.contrasena);
                command.Parameters.AddWithValue("@padreId", usuario.padre.id);

                command.ExecuteNonQuery();
            }
        }
    }
}

