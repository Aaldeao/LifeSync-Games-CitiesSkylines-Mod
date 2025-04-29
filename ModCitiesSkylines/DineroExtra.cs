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
    public class DineroExtra : ThreadingExtensionBase
    {
        // Tecla para activar el dinero extra
        private const KeyCode Boton_Dinero_Extra = KeyCode.F4;


        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {

            if (Input.GetKeyDown(Boton_Dinero_Extra))
            {
                AgregarDineroExtra();
            }
        }

        // Metodo que agrega dinero extra
        public void AgregarDineroExtra()
        {
            int dineroExtra = 100000;

            EconomyManager.instance.AddResource(
                EconomyManager.Resource.RewardAmount,
                dineroExtra,
                ItemClass.Service.None,
                ItemClass.SubService.None,
                ItemClass.Level.None);

            int dineroReal = dineroExtra / 100;

            // Muestra el mensaje cuando se agrega el dinero extra
            string dineroIcono = "ModCitiesSkylines.IconoDinero.png";
            MensajesView.MostrarMensajeDinero(
                "Dinero Extra",
                "Se han agregado $" + dineroReal + " a los ingresos públicos semanales",
                dineroIcono);
        }
    }

}
