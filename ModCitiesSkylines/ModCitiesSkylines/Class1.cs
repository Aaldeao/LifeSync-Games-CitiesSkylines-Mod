using System;
using System.IO;
using System.Net;
using System.Threading;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using ColossalFramework; // Biblioteca para Cities Skylines
using UnityEngine;
using ColossalFramework.UI; // Biblioteca de Unity para manipulacion del motor grafico y otros elementos


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

    //Clase que permite modificar la economia del juego mediante el uso de una tecla
    public class DineroExtra : ThreadingExtensionBase
    {
        private const KeyCode Boton_Dinero_Extra = KeyCode.L;

        // Variable compartida para mostrar mensajes
        private string mensajePendiente = null;
        private string tituloPendiente = null;

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (Input.GetKeyDown(Boton_Dinero_Extra))
            {
                AgregarDineroExtra();
            }

            // Mostrar mensaje de la conexion con la API de bGames
            if (!string.IsNullOrEmpty(mensajePendiente))
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                    tituloPendiente,
                    mensajePendiente,
                    false);

                mensajePendiente = null; // Limpiar después de mostrar
                tituloPendiente = null;
            }
        }

        public void AgregarDineroExtra()
        {
            int dineroExtra = 100000;

            EconomyManager.instance.AddResource(
                EconomyManager.Resource.PublicIncome,
                dineroExtra,
                ItemClass.Service.None,
                ItemClass.SubService.None,
                ItemClass.Level.None);

            int dineroReal = 1000;

            // Llamada a la API local
            HacerLlamadaApi();

            // Mostrar mensaje de dinero agregado
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                "Dinero Extra",
                "Se han agregado $" + dineroReal + " a los ingresos públicos semanales",
                false);
        }

        // Método para hacer la llamada a la API de bGames
        public void HacerLlamadaApi()
        {
            Thread t = new Thread(() =>
            {
                try
                {

                    string url = "http://localhost:3001"; 
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        tituloPendiente = "Conexión Exitosa";
                        mensajePendiente = "Respuesta del servidor: " + result;
                    }
                }
                catch (Exception ex) // Mensaje de error si no se puede conectar con la API
                {
                    tituloPendiente = "Error de conexión";
                    mensajePendiente = ex.Message;
                }
            });

            t.IsBackground = true;
            t.Start();
        }
    }
}