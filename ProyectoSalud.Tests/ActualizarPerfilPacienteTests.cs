using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;
using Interface.Dto;
using Interface.Dto.Request;
using System;

namespace ProyectoSalud.Tests
{
    [TestClass]
    public class ActualizarPerfilPacienteTests
    {
        private BOPerfilPaciente _boPerfilPaciente;

        [TestInitialize]
        public void Setup()
        {
            _boPerfilPaciente = new BOPerfilPaciente();
        }

        [TestMethod]
        public void ActualizarPerfilPaciente_CorrectData_ReturnsSuccess()
        {
            var request = new ModificarDatosPacienteRequestDto
            {
                perfilPacienteId = 1,
                perfilPaciente = new PerfilPacienteDto
                {
                    nombre = "Carlos",
                    fechaNacimiento = DateTime.Parse("2000-01-01"),
                    dni = "12345678",
                    genero = "M",
                    edadAnios = 22,
                    edadMeses = 6
                }
            };

            var response = _boPerfilPaciente.ModificarDatosPaciente(request);

            Assert.IsTrue(response.success);
            Assert.AreEqual("Perfil de paciente actualizado exitosamente.", response.message);
        }

        [TestMethod]
        public void ActualizarPerfilPaciente_MissingNombre_ReturnsError()
        {
            var request = new ModificarDatosPacienteRequestDto
            {
                perfilPacienteId = 1,
                perfilPaciente = new PerfilPacienteDto
                {
                    fechaNacimiento = DateTime.Parse("2000-01-01"),
                    dni = "12345678",
                    genero = "M",
                    edadAnios = 22,
                    edadMeses = 6
                }
            };

            var response = _boPerfilPaciente.ModificarDatosPaciente(request);

            Assert.IsFalse(response.success);
            Assert.IsTrue(response.message.Contains("Error al actualizar perfil de paciente"));
        }

        [TestMethod]
        public void ActualizarPerfilPaciente_InvalidFechaNacimiento_ReturnsError()
        {
            var request = new ModificarDatosPacienteRequestDto
            {
                perfilPacienteId = 1,
                perfilPaciente = new PerfilPacienteDto
                {
                    nombre = "Carlos",
                    fechaNacimiento = DateTime.Parse("2025-01-01"), // Fecha futura inválida
                    dni = "12345678",
                    genero = "M",
                    edadAnios = 22,
                    edadMeses = 6
                }
            };

            var response = _boPerfilPaciente.ModificarDatosPaciente(request);

            Assert.IsFalse(response.success);
            Assert.IsTrue(response.message.Contains("Error al actualizar perfil de paciente"));
        }

        [TestMethod]
        public void ActualizarPerfilPaciente_DatabaseError_ReturnsError()
        {
            // Simulación de error de base de datos
            var request = new ModificarDatosPacienteRequestDto
            {
                perfilPacienteId = -1, // Id inválido para provocar un error
                perfilPaciente = new PerfilPacienteDto
                {
                    nombre = "Carlos",
                    fechaNacimiento = DateTime.Parse("2000-01-01"),
                    dni = "12345678",
                    genero = "M",
                    edadAnios = 22,
                    edadMeses = 6
                }
            };

            var response = _boPerfilPaciente.ModificarDatosPaciente(request);

            Assert.IsFalse(response.success);
            Assert.IsTrue(response.message.Contains("Error al actualizar perfil de paciente"));
        }
    }
}
