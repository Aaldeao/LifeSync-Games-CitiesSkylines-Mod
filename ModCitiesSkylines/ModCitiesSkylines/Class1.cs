using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using ColossalFramework; // Biblioteca para Cities Skylines
using UnityEngine; // Biblioteca de Unity para manipulacion del motor grafico y otros elementos


namespace ModCitiesSkylines
{
    public class Class1 : IUserMod  // Permite que el mod sea reconocido por Cities Skylines
    {
        public string Name // Permite darle el nombre al mod
        {
            get { return "bGames-Cities:Skyline"; }
        }
        public string Description // Permite darle una descripción al mod
        {
            get { return "Un mod el cual incorpora las actividades del usuario en la experiencia de juego"; }
        }
    }

    //Clase que permite modificar la economia del juego
    public class DineroExtra : EconomyExtensionBase
    {
        // Metodo que actualiza la economia en el juego
        public override long OnUpdateMoneyAmount(long internalMoneyAmount)
        {
            return internalMoneyAmount + 1000000; // Modifica visualmente la economia del juego
        }
    }
}
