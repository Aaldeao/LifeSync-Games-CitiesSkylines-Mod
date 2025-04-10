using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using ColossalFramework.UI;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class DineroExtra : ThreadingExtensionBase
    {
        // Tecla para activar el dinero extra
        private const KeyCode Boton_Dinero_Extra = KeyCode.L;

        // Variables para almacenar el mensaje de la API
        private string mensajePendiente = null;
        private string tituloPendiente = null;

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (Input.GetKeyDown(Boton_Dinero_Extra))
            {
                AgregarDineroExtra();
            }

            // Mostrar mensaje de la conexion con la API de bGames
            if (!string.IsNullOrEmpty(mensajePendiente))
            {
                UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                    tituloPendiente,
                    mensajePendiente,
                    false);

                mensajePendiente = null;
                tituloPendiente = null;
            }
        }

        // Metodo que agrega dinero extra a los ingresos semanales
        public void AgregarDineroExtra()
        {
            int dineroExtra = 100000;

            EconomyManager.instance.AddResource(
                EconomyManager.Resource.PublicIncome,
                dineroExtra, 
                ItemClass.Service.None,
                ItemClass.SubService.None,
                ItemClass.Level.None);

            int dineroReal = 1000;

            HacerLlamadaApi();

            // Mostrar mensaje de dinero agregado
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                "Dinero Extra",
                "Se han agregado $" + dineroReal + " a los ingresos públicos semanales",
                false);
        }

        // Metodo para llamar a la API de bGames
        public void HacerLlamadaApi()
        {
           
            Thread t = new Thread(() =>
            {
                var bGames = new bGamesAPI();
                bGames.Llamar_bGames();

                // Mensaje de sobre la conexion con la API
                mensajePendiente = bGames.Mensaje;
                tituloPendiente = bGames.Titulo;
            });

            t.IsBackground = true;
            t.Start();
        }
    }
}
