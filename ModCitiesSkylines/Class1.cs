using System;
using System.IO;
using System.Net;
using System.Threading;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding


namespace ModCitiesSkylines
{
    // Permite que el mod sea reconocido por Cities Skylines
    public class Class1 : IUserMod
    {
        public string Name // Permite darle el nombre al mod
        {
            get { return "bGames-Cities:Skyline"; }
        }
        public string Description // Permite darle una descripción al mod
        {
            get { return "Convierte tus actividades en parte de la experiencia de juego"; }
        }
    }
}