using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using ColossalFramework.UI;
using UnityEngine;
using ColossalFramework.Plugins;

namespace ModCitiesSkylines
{
    public class DineroExtra : ThreadingExtensionBase
    {
        // Tecla para activar el dinero extra
        private const KeyCode Boton_Dinero_Extra = KeyCode.L;


        // Variables para almacenar el mensaje de la API
        private string mensajeAPI = null;
        private string tituloAPI = null;

        // Metodo que se ejecuta una vez por frame
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            // Muestra mensaje de la conexion con la API de bGames
            if (!string.IsNullOrEmpty(mensajeAPI))
            {
                Mensajes(tituloAPI, mensajeAPI);
                mensajeAPI = null;
                tituloAPI = null;
            }

            if (Input.GetKeyDown(Boton_Dinero_Extra))
            {
                AgregarDineroExtra();
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
                mensajeAPI = bGames.Mensaje;
                tituloAPI = bGames.Titulo;
            });

            t.IsBackground = true;
            t.Start();
        }

        private void Mensajes(string titulo, string mensaje)
        {
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(titulo, mensaje, false);
        }
    }

}
