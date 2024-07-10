using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;
using Interface.Dto;
using Interface.Dto.Request;
using Interface.Dto.Response;
using System;

namespace ProyectoSalud.Tests
{
    [TestClass]
    public class AgregarNotificacionTests
    {
        private BONotificacion _boNotificacion;

        [TestInitialize]
        public void Setup()
        {
            _boNotificacion = new BONotificacion();
        }

        [TestMethod]
        public void AgregarNotificacion_CorrectData_ReturnsSuccess()
        {
            var request = new AgregarNotificacionRequestDto
            {
                UsuarioId = 1,
                Descripcion = "Nueva notificación",
                FechaHora = DateTime.UtcNow
            };

            var response = _boNotificacion.AgregarNotificacion(request);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Notificación agregada exitosamente.", response.Message);
        }

        [TestMethod]
        public void AgregarNotificacion_MissingUsuarioId_ReturnsError()
        {
            var request = new AgregarNotificacionRequestDto
            {
                Descripcion = "Nueva notificación",
                FechaHora = DateTime.UtcNow
            };

            var response = _boNotificacion.AgregarNotificacion(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar notificación"));
        }

        [TestMethod]
        public void AgregarNotificacion_NullDescripcion_ReturnsError()
        {
            var request = new AgregarNotificacionRequestDto
            {
                UsuarioId = 1,
                FechaHora = DateTime.UtcNow
            };

            var response = _boNotificacion.AgregarNotificacion(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar notificación"));
        }

        [TestMethod]
        public void AgregarNotificacion_DatabaseError_ReturnsError()
        {
            // Simulación de error de base de datos
            var request = new AgregarNotificacionRequestDto
            {
                UsuarioId = -1, // Id inválido para provocar un error
                Descripcion = "Nueva notificación",
                FechaHora = DateTime.UtcNow
            };

            var response = _boNotificacion.AgregarNotificacion(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar notificación"));
        }
    }
}
