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
        public void InsertarPerfilPaciente(int usuarioId, PerfilPacienteDto perfilPaciente, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("INSERT INTO PerfilPacientes (nombre, fechaNacimiento, dni, genero, edadAnios, edadMeses, usuarioId) VALUES (@nombre, @fechaNacimiento, @dni, @genero, @edadAnios, @edadMeses, @usuarioId)", connection, transaction))
            {
                command.Parameters.AddWithValue("nombre", perfilPaciente.nombre);
                command.Parameters.AddWithValue("fechaNacimiento", perfilPaciente.fechaNacimiento);
                command.Parameters.AddWithValue("dni", perfilPaciente.dni);
                command.Parameters.AddWithValue("genero", perfilPaciente.genero);
                command.Parameters.AddWithValue("edadAnios", perfilPaciente.edadAnios);
                command.Parameters.AddWithValue("edadMeses", perfilPaciente.edadMeses);
                command.Parameters.AddWithValue("usuarioId", usuarioId);

                command.ExecuteNonQuery();
            }
        }

        public void ActualizarUsuario(int usuarioId, string email, string contrasena, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE Usuarios SET email = @Email, contraseña = @Contrasena, updatedAt = NOW() WHERE id = @UsuarioId", connection, transaction))
            {
                command.Parameters.AddWithValue("Email", email);
                command.Parameters.AddWithValue("Contrasena", contrasena);
                command.Parameters.AddWithValue("UsuarioId", usuarioId);

                command.ExecuteNonQuery();
            }
        }

        public void ActualizarPadre(PadreDto padre, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE Padres SET nombre = @Nombre, apellido = @Apellido, dni = @Dni, genero = @Genero, fechaNacimiento = @FechaNacimiento, updatedAt = NOW() WHERE id = @Id", connection, transaction))
            {
                command.Parameters.AddWithValue("Nombre", padre.nombre);
                command.Parameters.AddWithValue("Apellido", padre.apellido);
                command.Parameters.AddWithValue("Dni", padre.dni);
                command.Parameters.AddWithValue("Genero", padre.genero);
                command.Parameters.AddWithValue("FechaNacimiento", padre.fechaNacimiento);
                command.Parameters.AddWithValue("Id", padre.id);

                command.ExecuteNonQuery();
            }
        }

        public void ActualizarContrasena(string email, string nuevaContrasena, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE Usuarios SET contraseña = @NuevaContrasena, updatedAt = NOW() WHERE email = @Email", connection, transaction))
            {
                command.Parameters.AddWithValue("NuevaContrasena", nuevaContrasena);
                command.Parameters.AddWithValue("Email", email);

                command.ExecuteNonQuery();
            }
        }

        public int InsertarRegistroMedico(int perfilPacienteId, RegistroMedicoDto registroMedico, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("INSERT INTO RegistroMedicos (fecha, perfilPacienteId, createdAt, updatedAt) VALUES (@fecha, @perfilPacienteId, NOW(), NOW()) RETURNING id", connection, transaction))
            {
                command.Parameters.AddWithValue("fecha", registroMedico.fecha);
                command.Parameters.AddWithValue("perfilPacienteId", perfilPacienteId);

                return (int)command.ExecuteScalar();
            }
        }

        public void InsertarDatosMedicos(int registroMedicoId, DatosMedicosDto datosMedicos, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("INSERT INTO DatosMedicos (peso, talla, registroMedicoId, createdAt, updatedAt) VALUES (@peso, @talla, @registroMedicoId, NOW(), NOW())", connection, transaction))
            {
                command.Parameters.AddWithValue("peso", datosMedicos.peso);
                command.Parameters.AddWithValue("talla", datosMedicos.talla);
                command.Parameters.AddWithValue("registroMedicoId", registroMedicoId);

                command.ExecuteNonQuery();
            }
        }

        public void ActualizarPerfilPaciente(int perfilPacienteId, PerfilPacienteDto perfilPaciente, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE PerfilPacientes SET nombre = @nombre, fechaNacimiento = @fechaNacimiento, dni = @dni, genero = @genero, edadAnios = @edadAnios, edadMeses = @edadMeses, updatedAt = NOW() WHERE id = @perfilPacienteId", connection, transaction))
            {
                command.Parameters.AddWithValue("nombre", perfilPaciente.nombre);
                command.Parameters.AddWithValue("fechaNacimiento", perfilPaciente.fechaNacimiento);
                command.Parameters.AddWithValue("dni", perfilPaciente.dni);
                command.Parameters.AddWithValue("genero", perfilPaciente.genero);
                command.Parameters.AddWithValue("edadAnios", perfilPaciente.edadAnios);
                command.Parameters.AddWithValue("edadMeses", perfilPaciente.edadMeses);
                command.Parameters.AddWithValue("perfilPacienteId", perfilPacienteId);

                command.ExecuteNonQuery();
            }
        }

        public void EliminarPerfilPaciente(int perfilPacienteId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("DELETE FROM PerfilPacientes WHERE id = @perfilPacienteId", connection, transaction))
            {
                command.Parameters.AddWithValue("perfilPacienteId", perfilPacienteId);

                command.ExecuteNonQuery();
            }
        }

        public void ActualizarRegistroMedico(int registroMedicoId, RegistroMedicoDto registroMedico, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE RegistroMedicos SET fecha = @fecha, updatedAt = NOW() WHERE id = @registroMedicoId", connection, transaction))
            {
                command.Parameters.AddWithValue("fecha", registroMedico.fecha);
                command.Parameters.AddWithValue("registroMedicoId", registroMedicoId);

                command.ExecuteNonQuery();
            }
        }

        public void ActualizarDatosMedicos(int registroMedicoId, DatosMedicosDto datosMedicos, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE DatosMedicos SET peso = @peso, talla = @talla, updatedAt = NOW() WHERE registroMedicoId = @registroMedicoId", connection, transaction))
            {
                command.Parameters.AddWithValue("peso", datosMedicos.peso);
                command.Parameters.AddWithValue("talla", datosMedicos.talla);
                command.Parameters.AddWithValue("registroMedicoId", registroMedicoId);

                command.ExecuteNonQuery();
            }
        }

        public void EliminarRegistroMedico(int registroMedicoId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("DELETE FROM RegistroMedicos WHERE id = @registroMedicoId", connection, transaction))
            {
                command.Parameters.AddWithValue("registroMedicoId", registroMedicoId);

                command.ExecuteNonQuery();
            }
        }
    }


}

