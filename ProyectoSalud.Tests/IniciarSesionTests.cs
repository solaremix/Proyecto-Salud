using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;
using Interface.Dto.Request;
using Interface.Dto.Response;
using System;

namespace ProyectoSalud.Tests
{
    [TestClass]
    public class IniciarSesionTests
    {
        private BOAutenticacion _boAutenticacion;

        [TestInitialize]
        public void Setup()
        {
            _boAutenticacion = new BOAutenticacion();
        }

        [TestMethod]
        public void IniciarSesion_CorrectCredentials_ReturnsSuccess()
        {
            var request = new IniciarSesionRequestDto
            {
                email = "rodrigolinares912@gmail.com",
                contrasena = "password1"
            };

            var response = _boAutenticacion.IniciarSesion(request);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Inicio de sesión exitoso.", response.Message);
            Assert.IsNotNull(response.Usuario);
        }

        [TestMethod]
        public void IniciarSesion_IncorrectCredentials_ReturnsError()
        {
            var request = new IniciarSesionRequestDto
            {
                email = "usuario2@example.com",
                contrasena = "wrongpassword"
            };

            var response = _boAutenticacion.IniciarSesion(request);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("Email o contraseña incorrectos.", response.Message);
            Assert.IsNull(response.Usuario);
        }

        [TestMethod]
        public void IniciarSesion_EmptyEmail_ReturnsError()
        {
            var request = new IniciarSesionRequestDto
            {
                email = "",
                contrasena = "password1"
            };

            var response = _boAutenticacion.IniciarSesion(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al iniciar sesión"));
            Assert.IsNull(response.Usuario);
        }

        [TestMethod]
        public void IniciarSesion_EmptyPassword_ReturnsError()
        {
            var request = new IniciarSesionRequestDto
            {
                email = "usuario3@example.com",
                contrasena = ""
            };

            var response = _boAutenticacion.IniciarSesion(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al iniciar sesión"));
            Assert.IsNull(response.Usuario);
        }
    }
}
