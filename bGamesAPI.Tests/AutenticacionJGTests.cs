using Microsoft.VisualStudio.TestTools.UnitTesting;
using bGamesAPI;

namespace bGamesAPI.Tests
{
    [TestClass]
    public class AutenticacionJGTests
    {
        [DataTestMethod]
        [DataRow("test@test.cl", "asd123")]
        [DataRow("mail@mail.com", "asd123")]
        [DataRow("dev", "dev")]
        public void IniciarSesion_UsuarioValido_DeberiaIniciarSesionCorrectamente(string email, string password)
        {
            var resultado = AutenticacionJG.IniciarSesion(email, password);
            Assert.AreEqual("LifeSync Games", resultado.Titulo);
            Assert.AreEqual("Sesión iniciada correctamente.", resultado.Mensaje);
            Assert.IsNotNull(resultado.IdJugador);
        }

        [TestMethod]
        public void IniciarSesion_CorreoInvalido_DeberiaMostrarError()
        {
            var resultado = AutenticacionJG.IniciarSesion("invalido@correo.com", "cualquiera");
            Assert.AreEqual("LifeSync Games", resultado.Titulo);
            Assert.AreEqual("Correo no encontrado.", resultado.Mensaje);
        }

        [TestMethod]
        public void IniciarSesion_ContrasenaIncorrecta_DeberiaMostrarError()
        {
            var resultado = AutenticacionJG.IniciarSesion("test@test.cl", "incorrecta");
            Assert.AreEqual("LifeSync Games", resultado.Titulo);
            Assert.AreEqual("Contraseña incorrecta.", resultado.Mensaje);
        }
    }
}
