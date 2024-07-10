using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business;
using Interface.Dto;
using Interface.Dto.Request;

namespace ProyectoSalud.Tests
{
    [TestClass]
    public class AgregarUsuarioTests
    {
        private BOUsuario _boUsuario;

        [TestInitialize]
        public void Setup()
        {
            _boUsuario = new BOUsuario();
        }

        [TestMethod]
        public void AgregarUsuario_CorrectData_ReturnsSuccess()
        {
            var request = new RegistrarUsuarioRequestDto
            {
                usuario = new UsuarioDto
                {
                    email = "nuevo@ejemplo.com",
                    contrasena = "password123",
                    padre = new PadreDto { nombre = "Juan", apellido = "Pérez", dni = "12345678" }
                }
            };

            var response = _boUsuario.RegistrarUsuario(request);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Usuario agregado exitosamente.", response.Message);
        }

        [TestMethod]
        public void AgregarUsuario_MissingEmail_ReturnsError()
        {
            var request = new RegistrarUsuarioRequestDto
            {
                usuario = new UsuarioDto
                {
                    contrasena = "password123",
                    padre = new PadreDto { nombre = "Juan", apellido = "Pérez", dni = "12345678" }
                }
            };

            var response = _boUsuario.RegistrarUsuario(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar usuario"));
        }

        [TestMethod]
        public void AgregarUsuario_WeakPassword_ReturnsError()
        {
            var request = new RegistrarUsuarioRequestDto
            {
                usuario = new UsuarioDto
                {
                    email = "nuevo@ejemplo.com",
                    contrasena = "123", // Contraseña débil
                    padre = new PadreDto { nombre = "Juan", apellido = "Pérez", dni = "12345678" }
                }
            };

            var response = _boUsuario.RegistrarUsuario(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar usuario"));
        }

        [TestMethod]
        public void AgregarUsuario_DatabaseError_ReturnsError()
        {
            // Simulación de error de base de datos
            var request = new RegistrarUsuarioRequestDto
            {
                usuario = new UsuarioDto
                {
                    email = "error@ejemplo.com", // Email inválido para provocar un error
                    contrasena = "password123",
                    padre = new PadreDto { nombre = "Juan", apellido = "Pérez", dni = "12345678" }
                }
            };

            var response = _boUsuario.RegistrarUsuario(request);

            Assert.IsFalse(response.Success);
            Assert.IsTrue(response.Message.Contains("Error al agregar usuario"));
        }
    }
}
