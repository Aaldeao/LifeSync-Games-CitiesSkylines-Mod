using System;

using ColossalFramework.UI;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Tiempo : MonoBehaviour
    {
        private static DateTime tiempoAlIniciar = DateTime.MinValue; // Guarda el momento en que inicia la sesión

        private static bool advertencia = false; // Bandera para controlar que la advertencia solo se muestre una vez

        public static int inicioPenalizaciones = 60; // Minutos a partir de los cuales comienzan las penalizaciones

        // Método estático llamado al iniciar sesión o una nueva partida
        public static void PartidaIniciada()
        {
            tiempoAlIniciar = DateTime.Now; // Guarda el tiempo actual al iniciar la sesión
            advertencia = false; // Reinicia la bandera para mostrar la advertencia
            inicioPenalizaciones = 5; // Reinicia el contador para inicio de penalizaciones
        }

        // Método llamado automáticamente en cada frame.
        public void Update()
        {
            // Si el jugador no ha iniciado sesión o el tiempo no está inicializado, no hace nada
            if (!LoginPanel.inicioSesion || tiempoAlIniciar == DateTime.MinValue)
            {
                return;
            }

            // Calcula cuánto tiempo ha pasado desde que inició la sesión
            TimeSpan tiempoTranscurrido = DateTime.Now - tiempoAlIniciar;
            int minutos = (int)tiempoTranscurrido.TotalMinutes; // Convierte el tiempo transcurrido a minutos enteros

            // Si aún no se mostró la advertencia y el jugador lleva 60 minutos, muestra la advertencia
            if (!advertencia && minutos == 60)
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("LifeSync Games", "Has jugado los 60 minutos como maximo. Se recomienda hacer una pausa o cerrar el juego para evitar penalizaciones cada 30 minutos", false);
                advertencia = true; // Marca la advertencia como ya mostrada para no repetirla
            }

            // Si ya pasaron al menos 60 minutos desde el inicio, y han transcurrido 30 minutos desde la última penalización, aplica una nueva penalización
            if (minutos >= 60 && minutos - inicioPenalizaciones >= 30)
            {
                inicioPenalizaciones = minutos;  // Actualiza el contador para la próxima penalización
                Penalizacion.AplicarPenalizaciones();  // Llama a aplicar penalizaciones al jugador
            }
        }
    }
}
