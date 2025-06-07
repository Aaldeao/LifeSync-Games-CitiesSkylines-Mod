using System;

using ICities;

namespace ModCitiesSkylines
{
    // Clase que maneja la visualización de los puntos del jugador
    public class PuntoJG : ThreadingExtensionBase
    {
        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {   
            PuntosJG.Actualizar(realTimeDelta, simulationTimeDelta);
        }
    }
    
}
