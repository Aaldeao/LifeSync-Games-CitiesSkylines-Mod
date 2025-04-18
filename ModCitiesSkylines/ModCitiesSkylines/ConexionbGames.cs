using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class ConexionbGames : ThreadingExtensionBase
    {

        private const KeyCode Conexion_con_bGames = KeyCode.N;


        // Variables para almacenar el mensaje de la API
        private string mensajeAPI = null;
        private string tituloAPI = null;

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            // Muestra mensaje de la conexion con la API de bGames
            if (!string.IsNullOrEmpty(mensajeAPI))
            {
                Mensajes(tituloAPI, mensajeAPI);
                mensajeAPI = null;
                tituloAPI = null;
            }

            if (Input.GetKeyDown(Conexion_con_bGames))
            {
                HacerLlamadaApi();
            }
        }

        // Metodo para llamar a la API de bGames
        public void HacerLlamadaApi()
        {

            Thread t = new Thread(() =>
            {
                var bGames = new bGamesAPI();
                bGames.Llamar_bGames();

                // Mensaje de sobre la conexion con la API
                mensajeAPI = bGames.Mensaje;
                tituloAPI = bGames.Titulo;
            });

            t.IsBackground = true;
            t.Start();
        }

        private void Mensajes(string titulo, string mensaje)
        {
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(titulo, mensaje, false);
        }
    }
}
