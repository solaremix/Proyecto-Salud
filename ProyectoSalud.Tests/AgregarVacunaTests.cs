using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;
using Interface.Dto;
using Interface.Dto.Request;
using Interface.Dto.Response;
using System;
using System.Collections.Generic;

namespace ProyectoSalud.Tests
{
    [TestClass]
    public class AgregarVacunaTests
    {
        private BOVacuna _boVacuna;

        [TestInitialize]
        public void Setup()
        {
            _boVacuna = new BOVacuna();
        }

        [TestMethod]
        public void AgregarVacuna_CorrectData_ReturnsSuccess()
        {
            var request = new AgregarVacunaRequestDto
            {
                Vacuna = new VacunaDto
                {
                    Nombre = "Vacuna A",
                    Descripcion = "Descripción de la vacuna A",
                    Status = 1,
                    FechaAplicacion = DateTime.UtcNow,
                    PerfilPacienteId = 1,
                    PerfilPacienteNombre = "Paciente 1"
                },
                EsquemaVacunacion = new List<EsquemaVacunacionDto>
                {
                    new EsquemaVacunacionDto
                    {
                        VacunaId = 1,
                        PerfilPacienteId = 1,
                        FechaAplicacion = DateTime.UtcNow,
                        Estado = 0 // Pendiente
                    }
                }
            };

            var response = _boVacuna.AgregarVacuna(request);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Vacuna agregada exitosamente.", response.Message);
        }

        [TestMethod]
        public void AgregarVacuna_MissingNombre_ReturnsError()
        {
            var request = new AgregarVacunaRequestDto
            {
                Vacuna = new VacunaDto
                {
                    Descripcion = "Descripción de la vacuna A",
                    Status = 1,
                    FechaAplicacion = DateTime.UtcNow,
                    PerfilPacienteId = 1,
                    PerfilPacienteNombre = "Paciente 1"
                },
                EsquemaVacunacion = new List<EsquemaVacunacionDto>
                {
                    new EsquemaVacunacionDto
                    {
                        VacunaId = 1,
                        PerfilPacienteId = 1,
                        FechaAplicacion = DateTime.UtcNow,
                        Estado = 0 // Pendiente
                    }
                }
            };

            var response = _boVacuna.AgregarVacuna(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar vacuna"));
        }

        [TestMethod]
        public void AgregarVacuna_InvalidStatus_ReturnsError()
        {
            var request = new AgregarVacunaRequestDto
            {
                Vacuna = new VacunaDto
                {
                    Nombre = "Vacuna A",
                    Descripcion = "Descripción de la vacuna A",
                    Status = -1, // Status inválido
                    FechaAplicacion = DateTime.UtcNow,
                    PerfilPacienteId = 1,
                    PerfilPacienteNombre = "Paciente 1"
                },
                EsquemaVacunacion = new List<EsquemaVacunacionDto>
                {
                    new EsquemaVacunacionDto
                    {
                        VacunaId = 1,
                        PerfilPacienteId = 1,
                        FechaAplicacion = DateTime.UtcNow,
                        Estado = 0 // Pendiente
                    }
                }
            };

            var response = _boVacuna.AgregarVacuna(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar vacuna"));
        }

        [TestMethod]
        public void AgregarVacuna_DatabaseError_ReturnsError()
        {
            // Simulación de error de base de datos
            var request = new AgregarVacunaRequestDto
            {
                Vacuna = new VacunaDto
                {
                    Nombre = "Vacuna Error", // Nombre que provoca un error
                    Descripcion = "Descripción de la vacuna A",
                    Status = 1,
                    FechaAplicacion = DateTime.UtcNow,
                    PerfilPacienteId = -1, // Id inválido para provocar un error
                    PerfilPacienteNombre = "Paciente Error"
                },
                EsquemaVacunacion = new List<EsquemaVacunacionDto>
                {
                    new EsquemaVacunacionDto
                    {
                        VacunaId = -1, // Id inválido para provocar un error
                        PerfilPacienteId = -1, // Id inválido para provocar un error
                        FechaAplicacion = DateTime.UtcNow,
                        Estado = 0 // Pendiente
                    }
                }
            };

            var response = _boVacuna.AgregarVacuna(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar vacuna"));
        }
    }
}
