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
    public class PuntosJG
    {
        private static volatile bool mostrarPerfil = false; // Bandera compartida entre hilos para mostrar el perfil
        private static volatile bool mostrarError = false; // Bandera compartida entre hilos para mostrar errores

        private static int totalPuntos = 0; // Total de puntos del jugador
        private static string nombreUsuario = ""; // Nombre del usuario
        private static string mensajeError = null; // Mensaje de error
        private static string tituloError = null; // Título del error
        private static List<PerfilJGView.AtributoUsuario> atributosPerfil = new List<PerfilJGView.AtributoUsuario>();

        // Método que se ejecuta una vez por frame
        public static void Actualizar(float realTimeDelta, float simulationTimeDelta)
        {
            if (mostrarPerfil)
            {
                PerfilJGView.PerfilPanel(nombreUsuario, totalPuntos, atributosPerfil);
                mostrarPerfil = false;
            }

            if (mostrarError)
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel")
                    .SetMessage(tituloError ?? "Error", mensajeError ?? "Ha ocurrido un error.", false);
                mostrarError = false;
                mensajeError = null;
                tituloError = null;
            }
        }

        // Método para obtener los puntos del jugador en un hilo secundario
        public static void ObtenerPuntos()
        {
            Thread t = new Thread(() =>
            {
                var datosJ = AtributosJG.ObtenerPuntosdelJugador();

                if (datosJ.PuntosT.HasValue)
                {
                    atributosPerfil = datosJ.Atributos.Select(a => new PerfilJGView.AtributoUsuario
                    {
                        Atributo = a.Nombre,
                        Punto = a.Punto ?? 0
                    }).ToList();

                    totalPuntos = datosJ.PuntosT.Value;
                    nombreUsuario = LoginPanel.nombreJG;
                    mostrarPerfil = true; // señal para mostrar el perfil en el siguiente frame
                }
                else
                {
                    mensajeError = datosJ.Mensaje;
                    tituloError = datosJ.Titulo;
                    mostrarError = true;
                }
            });

            t.IsBackground = true;
            t.Start();
        }
    }
}