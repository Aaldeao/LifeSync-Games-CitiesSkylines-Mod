using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using ColossalFramework.UI;
using UnityEngine;
using ColossalFramework.Plugins;
using System.Threading;

using bGamesAPI;

namespace ModCitiesSkylines
{
    public class PuntosJG : ThreadingExtensionBase
    {
        // Tecla para ver los puntos del jugador
        private const KeyCode Puntos_de_Usuario = KeyCode.F3;

        // Variables para almacenar el mensaje de la API
        private string puntosAPI2 = null;
        private string tituloAPI2 = null;

        private int? puntos = null; // Variable para almacenar los puntos del jugador

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            // Muestra mensaje
            if (!string.IsNullOrEmpty(puntosAPI2))
            {
                Mensajes2(tituloAPI2, puntosAPI2);
                puntosAPI2 = null;
                tituloAPI2 = null;
            }

            if (Input.GetKeyDown(Puntos_de_Usuario))
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "ObtenerDatos() llamado");
                ObtenerPuntos();
            }
        }

        // Metodo para obtener los puntos del jugador en un hilo secundario
        public void ObtenerPuntos()
        {
            Thread t = new Thread(() =>
            {
                var datosJ = AtributosJG.ObtenerPuntosdelJugador();
                StringBuilder mensajePuntos = new StringBuilder();

                if (datosJ.Puntos.HasValue)
                {
                    mensajePuntos.AppendLine($"{datosJ.Mensaje}\n");

                    foreach (var atributo in datosJ.Atributos)
                    {
                        mensajePuntos.AppendLine($"{atributo.Nombre} : {(atributo.Punto == 0 ? "Sin puntos" : $"{atributo.Punto} {(atributo.Punto == 1 ? "pt" : "pts")}")}");
                    }
                }
                else
                {
                    mensajePuntos.AppendLine(datosJ.Mensaje); // mensaje de error o sin puntos
                }

                // Mensaje de sobre la conexion con la API
                puntosAPI2 = mensajePuntos.ToString();
                tituloAPI2 = datosJ.Titulo;
                puntos = datosJ.Puntos; // Almacena los puntos del jugador
            });

            t.IsBackground = true;
            t.Start();
        }

        private void Mensajes2(string titulo, string mensaje)
        {
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(titulo, mensaje, false);
        }
    }
}
