using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using UnityEngine;
using System.Threading;
using ColossalFramework.UI;

using bGamesAPI;

namespace ModCitiesSkylines
{
    public class ConexionbG : ThreadingExtensionBase
    {
        // Tecla para ver la conexion con la API de bGames
        private const KeyCode Conexion_con_bGames = KeyCode.F5;


        // Variables para almacenar el mensaje de la API
        private string mensajeAPI = null;
        private string tituloAPI = null;

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            // Muestra mensaje de la conexion con la API de bGames
            if (!string.IsNullOrEmpty(mensajeAPI))
            {
                MensajesConexion(tituloAPI, mensajeAPI);
                mensajeAPI = null;
                tituloAPI = null;
            }

            if (Input.GetKeyDown(Conexion_con_bGames))
            {
                HacerLlamadaApi();
            }
        }

        // Metodo para ver la conexion con la API de bGames en un hilo secundario
        public void HacerLlamadaApi()
        {

            Thread t = new Thread(() =>
            {
                var bGames = APIbGames.ConexionAPI();

                // Mensaje de sobre la conexion con la API
                mensajeAPI = bGames.Mensaje;
                tituloAPI = bGames.Titulo;
            });

            t.IsBackground = true;
            t.Start();
        }

        private void MensajesConexion(string titulo, string mensaje)
        {
            if (titulo == "Conexión Exitosa")
            {
                // Muestra un mensaje personalizado si la conexión es exitosa
                MsgView.PanelMSG(titulo, mensaje);
            }
            else
            {
                // Muestra un mensaje de error si la conexión falla
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(titulo, mensaje, false);
            }
        }

    }
}

