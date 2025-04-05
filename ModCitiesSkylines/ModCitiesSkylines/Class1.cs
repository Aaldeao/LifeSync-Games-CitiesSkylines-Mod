using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using ColossalFramework; // Biblioteca para Cities Skylines
using UnityEngine;
using ColossalFramework.UI; // Biblioteca de Unity para manipulacion del motor grafico y otros elementos


namespace ModCitiesSkylines
{
    public class Class1 : IUserMod  // Permite que el mod sea reconocido por Cities Skylines
    {
        public string Name // Permite darle el nombre al mod
        {
            get { return "bGames-Cities:Skyline"; }
        }
        public string Description // Permite darle una descripción al mod
        {
            get { return "Un mod el cual incorpora las actividades del usuario en la experiencia de juego"; }
        }
    }

    //Clase que permite modificar la economia del juego mediante el uso de una tecla
    public class DineroExtra : ThreadingExtensionBase
    {
        private const KeyCode Boton_Dinero_Extra = KeyCode.L; // Tecla que se usara para agregar el dinero extra al juego
        
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta) // Metodo que se ejecuta una vez por frame
        {
            if (Input.GetKeyDown(Boton_Dinero_Extra)) // Solo se ejecuta si se presiona la tecla definida
            {
                AgregarDineroExtra(); // Llama al metodo que agrega dinero extra
            }
        }

        public void AgregarDineroExtra() // Metodo que agrega dinero extra a los ingresos publicos semanales
        {
            int dineroExtra = 100000; // Cantidad de dinero extra que se va a agregar
            int dineroReal = 1000; // Cantidad de dinero que el juego agrega cuando se agrega el dineroExtra = 100000
            // Agrega dinero extra a los ingresos publicos semanales 
            EconomyManager.instance.AddResource(EconomyManager.Resource.PublicIncome, // Recurso al que se le agrega el dinero (Ingresos publicos semanales )
                dineroExtra, // Cantidad de dinero que se agrega
                ItemClass.Service.None,
                ItemClass.SubService.None,
                ItemClass.Level.None);

            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(
                "Dinero Extra", // Titulo de la ventana emergente
                "Se han agregado $" + dineroReal + " a los ingresos publicos semanales", // Mensaje de la ventana emergente
                false);
        }
    }
}