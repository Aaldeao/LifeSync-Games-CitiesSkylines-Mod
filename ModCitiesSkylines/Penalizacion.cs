using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bGamesAPI;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Penalizacion : LoadingExtensionBase
    {
        private GameObject penalizacionObject; // Objeto que representa la penalización en el juego

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame)
            {
                penalizacionObject = new GameObject("PenalizacionObject"); // Crea un nuevo objeto para la penalización
                penalizacionObject.AddComponent<Tiempo>(); // Añade el componente Tiempo al objeto
                Tiempo.PartidaIniciada(); // Llama al método para inicializar el tiempo al iniciar la partida
            }
        }

        public override void OnLevelUnloading()
        {
            if (penalizacionObject != null)
            {
                GameObject.Destroy(penalizacionObject); // Destruye el objeto de penalización al salir del juego

            }
        }

        public static void AplicarPenalizaciones()
        {
            int cantidadPenalizaciones = Tiempo.PenalizacionesPendientes; // Obtiene la cantidad de penalizaciones pendientes por aplicar

            // Si no hay penalizaciones pendiente spor aplicar, no hace nada
            if (cantidadPenalizaciones <= 0)
            {
                return;
            }

            // Obtiene los atributos actuales del jugador
            var atributos = new List<PerfilJGView.AtributoUsuario>(PuntosJG.atributosPerfil);

            var penalizadosEnCiclo = new HashSet<int>(); // Conjunto para rastrear los atributos penalizados en este ciclo
            var random = new System.Random(); // 

            // Aplica la penalizacion reduciendo un punto de un atributo aleatorio con más puntos
            for (int i = 0; i < cantidadPenalizaciones; i++)
            {
                int puntoMax = atributos.Max(a => a.Punto); // Busca la dimension con más puntos

                var dimensionesMax = atributos.Where(a => a.Punto == puntoMax && a.Punto > 0).ToList(); // Selecciona todos las dimensiones que tienen el máximo de puntos

                // Si no hay atributos con puntos para reducir, sale del bucle
                if (dimensionesMax.Count == 0)
                {
                    break;
                }

                PerfilJGView.AtributoUsuario dimensionSeleccionada; // Selecciona un atributo aleatorio de los que tienen el máximo de puntos

                if (dimensionesMax.Count == 1)
                {
                    dimensionSeleccionada = dimensionesMax[0]; // Si solo hay un atributo con el máximo de puntos, lo selecciona directamente
                }
                else
                {
                    if (penalizadosEnCiclo.Count >= dimensionesMax.Count)
                    {
                        penalizadosEnCiclo.Clear(); // 
                    }
                    var candidatos = dimensionesMax.Where(a => !penalizadosEnCiclo.Contains(a.IdAtributo)).ToList(); // Filtra los atributos que no han sido penalizados en este ciclo

                    if (candidatos.Count == 0)
                    {
                        break; // Si no hay candidatos, sale del bucle
                    }

                    dimensionSeleccionada = candidatos[random.Next(candidatos.Count)]; // Selecciona un atributo aleatorio de los 
                    penalizadosEnCiclo.Add(dimensionSeleccionada.IdAtributo); // Añade el atributo seleccionado al conjunto de penalizados en este ciclo
                }

                int puntoPenalizado = dimensionSeleccionada.Punto - 1;

                var respuesta = CanjearPts.EnviarCanje(
                    LoginPanel.idJugador.Value,
                    dimensionSeleccionada.IdAtributo,
                    puntoPenalizado
                );

                if (respuesta.Exito)
                {
                    dimensionSeleccionada.Punto = puntoPenalizado;

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
}