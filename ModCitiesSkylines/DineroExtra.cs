using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using UnityEngine;
using ColossalFramework.UI;
using ColossalFramework.Plugins;

namespace ModCitiesSkylines
{
    public class DineroExtra
    {
        // Metodo que agrega dinero extra gracias a los puntos que desea convertir/canjear/consumir el jugador
        public static void AgregarDineroExtra(int puntos)
        {
            int dineroExtra = puntos * 100000; // Multiplica los puntos por 100000 (que son 1000 debido a que el sistema de economía del juego utiliza valores en centavos).

            // Agrega el dinero extra calculado como una recompensa al jugador.
            EconomyManager.instance.AddResource(
                EconomyManager.Resource.RewardAmount,
                dineroExtra,
                ItemClass.Service.None,
                ItemClass.SubService.None,
                ItemClass.Level.None);
        }
    }
}
