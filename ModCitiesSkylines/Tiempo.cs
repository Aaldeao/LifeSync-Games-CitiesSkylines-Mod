using System;

using ColossalFramework.UI;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Tiempo : MonoBehaviour
    {
        private static DateTime tiempoAlIniciar; // Tiempo al iniciar la partida
        private static int penalizaciones= 0; // Numero de penalizaciones acumuladas por tiempo excedido
        private static bool advertencia = false; //Bandera para la advertencia de los 50 minutos jugados

        public static int PenalizacionesPendientes => penalizaciones;

        // Método que se llama al iniciar la partida para establecer el tiempo de inicio, reiniciar las penalizaciones y la advertencia.
        public static void PartidaIniciada()
        {
            tiempoAlIniciar = DateTime.Now; // Guarda el tiempo actual al iniciar
            penalizaciones = 0;
            advertencia = false; // Reinicia la bandera de advertencia
        }

        // Método llamado automáticamente en cada frame.
        public void Update()
        {
            // Verifica si el tiempo de inicio ha sido inicializado correctamente.
            if (tiempoAlIniciar == DateTime.MinValue)
            {
                return; // Si no se ha iniciado la partida, no hace nada
            }

            // Calcula el tiempo transcurrido desde que comenzó la partida
            TimeSpan tiempoTranscurrido = DateTime.Now - tiempoAlIniciar;
            int minutos = (int)tiempoTranscurrido.TotalMinutes;

            // Muestra una advertencia si el jugador ha jugado más de o igual a 50 minutos.
            if (!advertencia && minutos == 5)
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("Aviso de LifeSync Games", "Has jugado los 60 minutos como maximo. Se recomienda hacer una pausa o cerrar el juego para evitar penalizaciones cada 30 minutos", false);
                advertencia = true; // Marca la advertencia como ya mostrada para no repetirla
            }

            // Si se han superado los 60 minutos de juego, se comienza a aplicar penalizaciones
            if (minutos >= 5)
            {
                int tiempoExcedido = minutos - 5; // Calcula cuántos minutos han pasado desde la hora límite ( 1 hora = 60 minutos)
                int penalizacionesDetectadas = tiempoExcedido;  // Calcula cuántas penalizaciones deben aplicarse: 1 por cada 30 minutos excedidos

                // Solo aplica penalizaciones nuevas si hay más detectadas que las que ya se registraron
                if (penalizacionesDetectadas > penalizaciones)
                {
                    int nuevasPenalizaciones = penalizacionesDetectadas - penalizaciones;  // Calcula cuántas penalizaciones nuevas deben aplicarse
                    penalizaciones = penalizacionesDetectadas; // Actualiza el contador total de penalizaciones aplicadas

                    Penalizacion.AplicarPenalizaciones();
                }
            }
        }
    }
}
