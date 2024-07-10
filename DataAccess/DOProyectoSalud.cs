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
            _connectionString = ConfigurationManager.ConnectionStrings["ProyectoSalud"].ConnectionString;
        }

        public string ConnectionString => _connectionString;

        // Método para insertar un padre
        public int InsertarPadre(PadreDto padre, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = "INSERT INTO Padres (nombre, apellido, dni, genero, fechaNacimiento) VALUES (@nombre, @apellido, @dni, @genero, @fechaNacimiento) RETURNING id";
                command.Parameters.AddWithValue("@nombre", padre.nombre);
                command.Parameters.AddWithValue("@apellido", padre.apellido);
                command.Parameters.AddWithValue("@dni", padre.dni);
                command.Parameters.AddWithValue("@genero", padre.genero);
                command.Parameters.AddWithValue("@fechaNacimiento", padre.fechaNacimiento);

                return (int)command.ExecuteScalar();
            }
        }

        // Método para insertar un usuario
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

        // Método para obtener un usuario por email y contraseña
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

        // Método para insertar un perfil de paciente
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

        // Método para actualizar un usuario
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

        // Método para actualizar los datos de un padre
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

        // Método para actualizar la contraseña de un usuario
        public void ActualizarContrasena(string email, string nuevaContrasena, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE Usuarios SET contraseña = @NuevaContrasena, updatedAt = NOW() WHERE email = @Email", connection, transaction))
            {
                command.Parameters.AddWithValue("NuevaContrasena", nuevaContrasena);
                command.Parameters.AddWithValue("Email", email);

                command.ExecuteNonQuery();
            }
        }

        // Método para insertar un registro médico
        public int InsertarRegistroMedico(int perfilPacienteId, RegistroMedicoDto registroMedico, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("INSERT INTO RegistroMedicos (fecha, perfilPacienteId, createdAt, updatedAt) VALUES (@fecha, @perfilPacienteId, NOW(), NOW()) RETURNING id", connection, transaction))
            {
                command.Parameters.AddWithValue("fecha", registroMedico.fecha);
                command.Parameters.AddWithValue("perfilPacienteId", perfilPacienteId);

                return (int)command.ExecuteScalar();
            }
        }

        // Método para insertar datos médicos
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

        // Método para actualizar un perfil de paciente
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

        // Método para eliminar un perfil de paciente
        public void EliminarPerfilPaciente(int perfilPacienteId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("DELETE FROM PerfilPacientes WHERE id = @perfilPacienteId", connection, transaction))
            {
                command.Parameters.AddWithValue("perfilPacienteId", perfilPacienteId);

                command.ExecuteNonQuery();
            }
        }

        // Método para actualizar un registro médico
        public void ActualizarRegistroMedico(int registroMedicoId, RegistroMedicoDto registroMedico, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE RegistroMedicos SET fecha = @fecha, updatedAt = NOW() WHERE id = @registroMedicoId", connection, transaction))
            {
                command.Parameters.AddWithValue("fecha", registroMedico.fecha);
                command.Parameters.AddWithValue("registroMedicoId", registroMedicoId);

                command.ExecuteNonQuery();
            }
        }

        // Método para actualizar datos médicos
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

        // Método para eliminar un registro médico
        public void EliminarRegistroMedico(int registroMedicoId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("DELETE FROM RegistroMedicos WHERE id = @registroMedicoId", connection, transaction))
            {
                command.Parameters.AddWithValue("registroMedicoId", registroMedicoId);

                command.ExecuteNonQuery();
            }
        }

        // Método para obtener perfiles por ID de usuario
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

        // Método para obtener registros médicos por ID de perfil de paciente
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

        public int InsertarVacuna(VacunaDto vacuna, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("INSERT INTO Vacunas (nombre, descripcion, status) VALUES (@nombre, @descripcion, @status) RETURNING id", connection, transaction))
            {
                command.Parameters.AddWithValue("nombre", vacuna.Nombre);
                command.Parameters.AddWithValue("descripcion", vacuna.Descripcion);
                command.Parameters.AddWithValue("status", vacuna.Status);
                return (int)command.ExecuteScalar();
            }
        }

        // Método para actualizar una vacuna
        public void ActualizarVacuna(VacunaDto vacuna, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE Vacunas SET nombre = @nombre, descripcion = @descripcion, status = @status, updatedAt = NOW() WHERE id = @id", connection, transaction))
            {
                command.Parameters.AddWithValue("nombre", vacuna.Nombre);
                command.Parameters.AddWithValue("descripcion", vacuna.Descripcion);
                command.Parameters.AddWithValue("status", vacuna.Status);
                command.Parameters.AddWithValue("id", vacuna.Id);
                command.ExecuteNonQuery();
            }
        }

        // Método para eliminar una vacuna
        public void EliminarVacuna(int vacunaId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("DELETE FROM Vacunas WHERE id = @id", connection, transaction))
            {
                command.Parameters.AddWithValue("id", vacunaId);
                command.ExecuteNonQuery();
            }
        }

        // Método para insertar un esquema de vacunación
        public void InsertarEsquemaVacunacion(EsquemaVacunacionDto esquema, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            if (esquema == null)
            {
                throw new ArgumentNullException(nameof(esquema), "El esquema de vacunación no puede ser nulo.");
            }

            using (var command = new NpgsqlCommand("INSERT INTO EsquemaVacunacion (vacunaId, perfilPacienteId, fechaAplicacion, estado) VALUES (@vacunaId, @perfilPacienteId, @fechaAplicacion, @estado)", connection, transaction))
            {
                command.Parameters.AddWithValue("vacunaId", esquema.VacunaId);
                command.Parameters.AddWithValue("perfilPacienteId", esquema.PerfilPacienteId);
                command.Parameters.AddWithValue("fechaAplicacion", esquema.FechaAplicacion);
                command.Parameters.AddWithValue("estado", esquema.Estado);
                command.ExecuteNonQuery();
            }
        }

        // Método para actualizar un esquema de vacunación
        public void ActualizarEsquemaVacunacion(EsquemaVacunacionDto esquema, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("UPDATE EsquemaVacunacion SET vacunaId = @vacunaId, perfilPacienteId = @perfilPacienteId, fechaAplicacion = @fechaAplicacion, estado = @estado, updatedAt = NOW() WHERE id = @id", connection, transaction))
            {
                command.Parameters.AddWithValue("vacunaId", esquema.VacunaId);
                command.Parameters.AddWithValue("perfilPacienteId", esquema.PerfilPacienteId);
                command.Parameters.AddWithValue("fechaAplicacion", esquema.FechaAplicacion);
                command.Parameters.AddWithValue("estado", esquema.Estado);
                command.Parameters.AddWithValue("id", esquema.Id);
                command.ExecuteNonQuery();
            }
        }

        // Método para eliminar un esquema de vacunación
        public void EliminarEsquemaVacunacion(int esquemaVacunacionId, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand("DELETE FROM EsquemaVacunacion WHERE id = @id", connection, transaction))
            {
                command.Parameters.AddWithValue("id", esquemaVacunacionId);
                command.ExecuteNonQuery();
            }
        }

        public List<VacunaDto> ObtenerVacunasPorPerfilPaciente(int perfilPacienteId)
        {
            List<VacunaDto> vacunas = new List<VacunaDto>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                SELECT 
                    vacunas.id,
                    vacunas.nombre,
                    vacunas.descripcion,
                    vacunas.status,
                    esquemavacunacion.fechaAplicacion,
                    perfilpacientes.id AS perfilpaciente_id,
                    perfilpacientes.nombre AS perfilpaciente_nombre
                FROM 
                    vacunas
                JOIN 
                    esquemavacunacion ON vacunas.id = esquemavacunacion.vacunaid
                JOIN 
                    perfilpacientes ON esquemavacunacion.perfilpacienteid = perfilpacientes.id
                WHERE 
                    perfilpacientes.id = @perfilPacienteId";

                    command.Parameters.AddWithValue("@perfilPacienteId", perfilPacienteId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vacunas.Add(new VacunaDto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                                Status = reader.GetInt32(reader.GetOrdinal("status")),
                                FechaAplicacion = reader.GetDateTime(reader.GetOrdinal("fechaAplicacion")),
                                PerfilPacienteId = reader.GetInt32(reader.GetOrdinal("perfilpaciente_id")),
                                PerfilPacienteNombre = reader.GetString(reader.GetOrdinal("perfilpaciente_nombre"))
                            });
                        }
                    }
                }
            }

            return vacunas;
        }


        public void InsertarNotificacion(NotificacionDto notificacion, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO Notificaciones (usuarioId, descripcion, fechaHora, createdAt, updatedAt) VALUES (@usuarioId, @descripcion, @fechaHora, @createdAt, @updatedAt)", connection, transaction))
            {
                cmd.Parameters.AddWithValue("usuarioId", notificacion.UsuarioId);
                cmd.Parameters.AddWithValue("descripcion", notificacion.Descripcion);
                cmd.Parameters.AddWithValue("fechaHora", notificacion.FechaHora);
                cmd.Parameters.AddWithValue("createdAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("updatedAt", DateTime.UtcNow);
                cmd.ExecuteNonQuery();
            }
        }

        public List<NotificacionDto> ObtenerNotificacionesPorUsuario(int usuarioId, NpgsqlConnection connection)
        {
            var notificaciones = new List<NotificacionDto>();

            using (var cmd = new NpgsqlCommand("SELECT id, usuarioId, descripcion, fechaHora FROM Notificaciones WHERE usuarioId = @usuarioId ORDER BY fechaHora DESC", connection))
            {
                cmd.Parameters.AddWithValue("usuarioId", usuarioId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var notificacion = new NotificacionDto
                        {
                            Id = reader.GetInt32(0),
                            UsuarioId = reader.GetInt32(1),
                            Descripcion = reader.GetString(2),
                            FechaHora = reader.GetDateTime(3)
                        };

                        notificaciones.Add(notificacion);
                    }
                }
            }

            return notificaciones;
        }
    }
}
