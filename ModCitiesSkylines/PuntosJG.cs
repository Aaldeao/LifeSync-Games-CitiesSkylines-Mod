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

        private volatile bool mostrarPerfil = false; // Bandera compartida entre hilos para mostrar el perfil
        private volatile bool mostrarError = false; // Bandera compartida entre hilos para mostrar errores

        private int totalPuntos = 0; // Total de puntos del jugador
        private string nombreUsuario = ""; // Nombre del usuario
        private string mensajeError = null; // Mensaje de error
        private string tituloError = null; // Título del error
        private List<PerfilJGView.AtributoUsuario> atributosPerfil = new List<PerfilJGView.AtributoUsuario>();

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (Input.GetKeyDown(Puntos_de_Usuario))
            {
                ObtenerPuntos();
            }

            // Mostrar el perfil en el hilo principal
            if (mostrarPerfil)
            {
                PerfilJGView.PerfilPanel(nombreUsuario, totalPuntos, atributosPerfil);
                mostrarPerfil = false;
            }

            // Mostrar errores en el hilo principal
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
        public void ObtenerPuntos()
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