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

        public UsuarioDto ObtenerUsuarioPorEmailYContrasena(string email, string contrasena, NpgsqlConnection connection)
        {
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT u.email, u.contraseña, p.nombre, p.apellido, p.dni, p.genero, p.fechaNacimiento " +
                                      "FROM Usuarios u JOIN Padres p ON u.padreId = p.id " +
                                      "WHERE u.email = @correo AND u.contraseña = @contrasena";
                command.Parameters.AddWithValue("@correo", email);
                command.Parameters.AddWithValue("@contrasena", contrasena);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UsuarioDto
                        {
                            email = reader.GetString(0),
                            contrasena = reader.GetString(1),
                            padre = new PadreDto
                            {
                                nombre = reader.GetString(2),
                                apellido = reader.GetString(3),
                                dni = reader.GetString(4),
                                genero = reader.GetString(5),
                                fechaNacimiento = reader.GetDateTime(6)
                            }
                        };
                    }
                }
            }
            return null;
        }
    }


}

