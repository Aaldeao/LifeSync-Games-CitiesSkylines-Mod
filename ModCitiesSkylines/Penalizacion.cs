using System;
using System.Collections.Generic;
using System.Linq;

using bGamesAPI;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Penalizacion : LoadingExtensionBase
    {
        private GameObject penalizacionObject; // Objeto que representa la penalización en el juego

        // Método que se llama cuando se carga un nivel (nuevo o guardado)
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame)
            {
                penalizacionObject = new GameObject("PenalizacionObject"); // Crea un nuevo objeto para la penalización
                penalizacionObject.AddComponent<Tiempo>(); // Añade el componente Tiempo al objeto
            }
        }

        public override void OnLevelUnloading()
        {
            if (penalizacionObject != null)
            {
                GameObject.Destroy(penalizacionObject); // Destruye el objeto de penalización al salir del juego

            }

            Tiempo.LimpiarTiempo();
        }

        // Metodo para aplicar penalizaciones al jugador
        public static void AplicarPenalizaciones()
        {
            int cantidadPenalizacion = 1; // Cantidad de puntos que se van a restar (por ahora, 1)

            // Si no hay puntos a restar, no hace nada
            if (cantidadPenalizacion <= 0)
            {
                return;
            }

            // Obtiene los atributos actuales del jugador
            var atributos = new List<PerfilJGView.AtributoUsuario>(PuntosJG.atributosPerfil);

            int puntoMax = atributos.Max(a => a.Punto); // Busca el valor de punto más alto entre todas las dimensiones del jugador
            var dimensionesMax = atributos.Where(a => a.Punto == puntoMax && a.Punto > 0).ToList();  // Filtra solo las dimensiones que tengan ese valor máximo y que tengan más de 0 puntos

            // Si no hay ninguna dimensión con puntos, no se puede penalizar // mostrar mensaje sobre esto
            if (dimensionesMax.Count == 0)
            {
                return;
            }

            var random = new System.Random(); // Crea un objeto Random para seleccionar una dimension al azar entre los máximos
            var dimensionSeleccionada = dimensionesMax[random.Next(dimensionesMax.Count)]; // Selecciona una dimensión al azar de entre las que tienen el valor máximo

            int puntoPenalizado = dimensionSeleccionada.Punto - cantidadPenalizacion; // Resta 1 punto a la dimensión seleccionada

            // Envia el nuevo valor de puntos penalizados de la dimension a la API
            var respuesta = Puntos.EnviarCanje(
                LoginPanel.idJugador.Value,
                dimensionSeleccionada.IdAtributo,
                puntoPenalizado
            );

            // Si la respuesta fue exitosa (penalización aplicada)
            if (respuesta.Exito)
            {
                dimensionSeleccionada.Punto = puntoPenalizado;

                PerfilJGView.OcultarPerfil(); // Oculta el perfil del jugador al aplicar la penalización

                if (CanjePuntosPanel.EstaAbierto())
                {
                    CanjePuntosPanel.CerrarPanel(); // Cierra el panel de canje de puntos si está abierto
                }

                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                        "Penalización aplicada",
                        $"Se ha descontado 1 punto de la dimensión: {dimensionSeleccionada.Atributo}",
                        false
                    );
            }
            else
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                    respuesta.Titulo,
                    respuesta.Mensaje,
                    false
                );
            }
        }
    }
}