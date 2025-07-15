using Microsoft.VisualStudio.TestTools.UnitTesting;
using bGamesAPI;

namespace bGamesAPI.Tests
{
    [TestClass]
    public class Conexion_bGTests
    {
        [TestMethod]
        public void ConexionAPI_DeberiaRetornarMensajeConfirmado()
        {
            // Act
            var resultado = APIbGames.ConexionAPI();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("LifeSync Games", resultado.Titulo);
            Assert.IsTrue(resultado.Mensaje.Contains("confirmada exitosamente"));
        }
    }
}
