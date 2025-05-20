using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace ModCitiesSkylines
{
    // Clase que maneja la visualización de los puntos del jugador
    public class PuntoJG : ThreadingExtensionBase
    {

        // Tecla para ver los puntos del jugador
        //private const KeyCode Puntos_de_Usuario = KeyCode.F3;
        /* --> SI DESO QUE FUNCIONE LA TECLA F3 DEBE IR DENTRO DEL METODO ONUPDATE
        if (Input.GetKeyDown(Puntos_de_Usuario))
        {
            PuntosJG.ObtenerPuntos();
        }
        */

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {   
            PuntosJG.Actualizar(realTimeDelta, simulationTimeDelta);
        }


    }
    
}
