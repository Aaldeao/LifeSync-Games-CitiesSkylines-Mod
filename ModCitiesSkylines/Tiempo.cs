using System;

using ColossalFramework.UI;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Tiempo : MonoBehaviour
    {
        private static DateTime tiempoAlIniciar = DateTime.MinValue; // Guarda el momento en que inicia la sesión

        private static int ultimaAdvertencia = 0; // Guarda el minuto en que se mostró la última advertencia
        public static int inicioPenalizaciones = 60; // Minutos a partir de los cuales comienzan las penalizaciones

        // Método estático llamado al iniciar sesión o una nueva partida
        public static void LoginIniciado()
        {
            tiempoAlIniciar = DateTime.Now; // Guarda el tiempo actual al iniciar la sesión
            inicioPenalizaciones = 60; // Reinicia el contador para inicio de penalizaciones
            ultimaAdvertencia = 0; // Reinicia el contador de la última advertencia
        }

        // Método llamado automáticamente en cada frame.
        public void Update()
        {
            // Si el tiempo no está inicializado, no hace nada
            if (tiempoAlIniciar == DateTime.MinValue)
            {
                return;
            }

            // Calcula cuánto tiempo ha pasado desde que inició la sesión
            TimeSpan tiempoTranscurrido = DateTime.Now - tiempoAlIniciar;
            int minutos = (int)tiempoTranscurrido.TotalMinutes; // Convierte el tiempo transcurrido a minutos enteros

            int horasJugadas = minutos / 60; // Calcula las horas jugadas dividiendo los minutos por 60

            // Cada 1 hora jugada, muestra una advertencia
            if (horasJugadas > 0 && horasJugadas > ultimaAdvertencia)
            {
                ultimaAdvertencia = horasJugadas;

                if (LoginPanel.inicioSesion) // Si el jugador está logueado, muestra un mensaje de advertencia
                {
                    UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                        "LifeSync Games", $"Has jugado por {horasJugadas} hora(s). Se recomienda hacer una pausa cerrando el juego para evitar penalizaciones cada 30 minutos", false);
                }
                else // Si el jugador no está logueado, muestra un mensaje de advertencia más general
                {
                    UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                        "LifeSync Games", $"Has jugado por {horasJugadas} hora(s). Se recomienda hacer una pausa cerrando el juego", false);
                }
            }


            // Si esta logueado, ademas ya pasaron al menos 60 minutos desde el inicio de sesion y han pasado 30 minutos desde la última penalización, aplica la penalización
            if (LoginPanel.inicioSesion && minutos >= 60 && minutos - inicioPenalizaciones >= 30)
            {
                inicioPenalizaciones = minutos;  // Actualiza el contador para la próxima penalización
                Penalizacion.AplicarPenalizaciones();  // Llama a aplicar penalizaciones al jugador
            }
        }
    }
}
