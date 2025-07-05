using System;

using ColossalFramework.UI;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Tiempo : MonoBehaviour
    {
        private static DateTime tiempoAlIniciar = DateTime.MinValue; // Guarda el momento en que inicia la sesión
        private static int ultimaAdvertencia = 0; // Guarda el minuto en que se mostró la última advertencia

        /* Activar para el Mod Clone
        private static bool advertencia = false; // Bandera para controlar que la advertencia solo se muestre una vez
        public static int inicioPenalizaciones = 60; // Minutos a partir de los cuales comienzan las penalizaciones
        */

        // Método estático llamado al iniciar sesión o una nueva partida
        public static void LoginIniciado()
        {
            tiempoAlIniciar = DateTime.Now; // Guarda el tiempo actual al iniciar la sesión
            ultimaAdvertencia = 0; // Reinicia el contador de advertencias

            /* Activar para el Mod Clone
            inicioPenalizaciones = 60; // Reinicia el contador donde comienzan las penalizaciones
            advertencia = false;  // Reinicia la bandera de advertencia
            */
        }


        public static void LimpiarTiempo()
        {
            tiempoAlIniciar = DateTime.MinValue; // Restablece el tiempo al valor mínimo
            ultimaAdvertencia = 0; // Reinicia el contador de advertencias

            /* Activar para el Mod Clone
            inicioPenalizaciones = 60; // Reinicia el contador donde comienzan las penalizaciones
            advertencia = false; // Reinicia la bandera de advertencia
            */

            // Ademas cerramos sesion y limpiamos los campos de texto del login
            LoginPanel.idJugador = null;  // Restablece el ID del jugador a null
            if (LoginPanel.usuario != null) LoginPanel.usuario.text = "";  // Limpia el campo de texto del correo del usuario en el login
            if (LoginPanel.password != null) LoginPanel.password.text = "";  // Limpia el campo de texto de la contraseña en el login
        }


        // Método llamado automáticamente en cada frame.
        public void Update()
        {
            // Si el tiempo no está inicializado o no se ha iniciado sesión, no hace nada 
            if (!LoginPanel.inicioSesion || tiempoAlIniciar == DateTime.MinValue)
            {
                return;
            }

            // Calcula cuánto tiempo ha pasado desde que inició la sesión
            TimeSpan tiempoTranscurrido = DateTime.Now - tiempoAlIniciar;
            int minutos = (int)tiempoTranscurrido.TotalMinutes; // Convierte el tiempo transcurrido a minutos enteros

            int horasJugadas = minutos / 60; // Calcula las horas jugadas (cada 60 minutos es una 1 hora de juego)

            if (horasJugadas > 0 && horasJugadas > ultimaAdvertencia)
            {
                ultimaAdvertencia = horasJugadas;

                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("LifeSync Games", $"Has jugado por {horasJugadas} hora(s). Se recomienda hacer una pausa cerrando el juego.", false);
            }

            /* Activar para el Mod Clone
            // Muestra un mensaje de advertencia si han pasado 60 minutos 
            if (!advertencia && minutos == 60)
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage("LifeSync Games", "Has jugado los 60 minutos como maximo. Se recomienda hacer una pausa o cerrar el juego para evitar penalizaciones cada 30 minutos", false);
                advertencia = true; // Marca la advertencia como ya mostrada para no repetirla
            }

            
            // Si esta logueado, ademas ya pasaron al menos 60 minutos desde el inicio de sesion y han pasado 30 minutos desde la última penalización, aplica la penalización
            if (LoginPanel.inicioSesion && minutos >= 60 && minutos - inicioPenalizaciones >= 30)
            {
                inicioPenalizaciones = minutos;  // Actualiza el contador para la próxima penalización
                Penalizacion.AplicarPenalizaciones();  // Llama a aplicar penalizaciones al jugador
            }
            */

        }
    }
}
