using Microsoft.VisualStudio.TestTools.UnitTesting;
using bGamesAPI;

namespace bGamesAPI.Tests
{
    [TestClass]
    public class PuntosTests
    {
        [DataTestMethod]
        [DataRow(0, 0, 10)]
        [DataRow(1, 3, 50)]
        [DataRow(2, 4, 99)]
        public void EnviarCanje_ParametrosValidos_DeberiaSerExitoso(int idJugador, int idAtributo, int nuevoValor)
        {
            var resultado = Puntos.EnviarCanje(idJugador, idAtributo, nuevoValor);
            Assert.IsTrue(resultado.Exito);
            Assert.AreEqual("LifeSync Games", resultado.Titulo);
            Assert.AreEqual("Canje realizado correctamente.", resultado.Mensaje);
        }
    }
}
