using Interface.Dto;
using Npgsql;
using System;
using System.Collections.Generic;
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
                command.CommandText = "SELECT u.id, u.email, u.contraseña, p.id, p.nombre, p.apellido, p.dni, p.genero, p.fechaNacimiento " +
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
                            id = reader.GetInt32(0),        // ID del usuario
                            email = reader.GetString(1),    // Email del usuario
                            contrasena = reader.GetString(2), // Contraseña del usuario
                            padre = new PadreDto
                            {
                                id = reader.GetInt32(3),    // ID del padre
                                nombre = reader.GetString(4), // Nombre del padre
                                apellido = reader.GetString(5), // Apellido del padre
                                dni = reader.GetString(6),    // DNI del padre
                                genero = reader.GetString(7), // Género del padre
                                fechaNacimiento = reader.GetDateTime(8) // Fecha de nacimiento del padre
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

        public List<PerfilPacienteDto> ObtenerPerfilesPorUsuarioId(int usuarioId, NpgsqlConnection connection)
        {
            var perfiles = new List<PerfilPacienteDto>();

            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT p.id, p.nombre, p.dni, p.fechaNacimiento, p.genero, p.edadAnios, p.edadMeses " +
                                      "FROM PerfilPacientes p " +
                                      "JOIN Usuarios u ON p.usuarioId = u.id " +
                                      "WHERE u.id = @usuarioId";
                command.Parameters.AddWithValue("@usuarioId", usuarioId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var perfil = new PerfilPacienteDto
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            dni = reader.GetString(reader.GetOrdinal("dni")),
                            fechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fechaNacimiento")),
                            genero = reader.GetString(reader.GetOrdinal("genero")),
                            edadAnios = reader.GetInt32(reader.GetOrdinal("edadAnios")),
                            edadMeses = reader.GetInt32(reader.GetOrdinal("edadMeses"))
                        };
                        perfiles.Add(perfil);
                    }
                }
            }

            return perfiles;
        }

        public List<RegistroMedicoDto> ObtenerRegistrosMedicosPorPerfilPacienteId(int perfilPacienteId, NpgsqlConnection connection)
        {
            var registros = new List<RegistroMedicoDto>();

            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT r.id AS registroMedicoId, r.fecha, d.peso, d.talla " +
                                      "FROM RegistroMedicos r " +
                                      "JOIN DatosMedicos d ON r.id = d.registroMedicoId " +
                                      "WHERE r.perfilPacienteId = @perfilPacienteId";
                command.Parameters.AddWithValue("@perfilPacienteId", perfilPacienteId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var registro = new RegistroMedicoDto
                        {
                            registroMedicoId = reader.GetInt32(reader.GetOrdinal("registroMedicoId")),
                            fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                            datos = new DatosMedicosDto
                            {
                                registroMedicoId = reader.GetInt32(reader.GetOrdinal("registroMedicoId")),
                                peso = Convert.ToSingle(reader["peso"]),
                                talla = Convert.ToSingle(reader["talla"])
                            }
                        };
                        registros.Add(registro);
                    }
                }
            }

            return registros;
        }
    }
}




