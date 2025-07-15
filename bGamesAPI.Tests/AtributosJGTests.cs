using Microsoft.VisualStudio.TestTools.UnitTesting;
using bGamesAPI;
using ModCitiesSkylines;
using System.Collections.Generic;

namespace bGamesAPI.Tests
{
    [TestClass]
    public class AtributosJGTests
    {
        [TestMethod]
        public void ObtenerPuntosdelJugador_ConIdValido_ValidaCantidadEsperada()
        {
            var esperadoPorJugador = new Dictionary<int, int>
             {
                    { 0, 5 }, // Jugador id 0 tiene 5 atributos
                    { 1, 1 }, // Jugador id 1 tiene 1 atributo
                    { 2, 5 }  // Jugador id 2 tiene 5 atributos
             };

            foreach (var kvp in esperadoPorJugador)
            {
                LoginPanel.idJugador = kvp.Key;

                var resultado = AtributosJG.ObtenerPuntosdelJugador();

                Assert.IsNotNull(resultado);
                Assert.AreEqual(kvp.Value, resultado.Atributos.Count, $"El jugador {kvp.Key} debería tener {kvp.Value} atributos.");
                Assert.IsTrue(resultado.PuntosT.HasValue, $"El jugador {kvp.Key} debería tener PuntosT definido.");
            }
        }


        [TestMethod]
        public void ObtenerPuntosdelJugador_SinSesionIniciada_RetornaMensajeDeError()
        {
            // Arrange
            LoginPanel.idJugador = null;

            // Act
            var resultado = AtributosJG.ObtenerPuntosdelJugador();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Mensaje.Contains("No se ha iniciado sesión"));
        }
    }
}
