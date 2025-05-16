using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Puntos_JG : ThreadingExtensionBase
    {
        // Tecla para ver los puntos del jugador
        private const KeyCode Puntos_de_Usuario = KeyCode.F3;

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (Input.GetKeyDown(Puntos_de_Usuario))
            {
                PuntosJG.ObtenerPuntos();
            }

            PuntosJG.Actualizar(realTimeDelta, simulationTimeDelta);
        }


    }
}
