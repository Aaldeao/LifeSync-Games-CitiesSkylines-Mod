using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Referencias de la biblioteca de Cities Skylines
using ICities; // API de Cities Skylines para modding
using ColossalFramework; // Biblioteca para Cities Skylines
using UnityEngine; // Biblioteca de Unity para manipulacion del motor grafico y otros elementos


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
        private bool dineroExtra_Activado = false; // Bandera que indica si el dinero extra esta activado o no

        // Metodo que se ejecuta antes de cada frame de simulacion
        public override void OnBeforeSimulationFrame()

        {
            if (Input.GetKeyDown(Boton_Dinero_Extra)) // Solo se ejecuta si se presiona la tecla definida
            {
                if (!dineroExtra_Activado) // Si el dinero extra no esta activado
                {
                    AgregarDineroExtra(); // Llama al metodo que agrega dinero extra
                    dineroExtra_Activado = true; // Cambia la bandera a activado
                }
                else // Si el dinero extra ya esta activado
                {
                    dineroExtra_Activado = false; // Cambia la bandera a desactivado
                }
            }
        }
     

        public void AgregarDineroExtra() // Metodo que agrega dinero extra a los ingresos semanales publicos
        {
            int dineroExtra = 100000; // Cantidad de dinero extra que se va a agregar

            // Agrega dinero extra a los ingresos semanales publicos
            EconomyManager.instance.AddResource(EconomyManager.Resource.PublicIncome, // Recurso al que se le agrega el dinero (Ingresos semanales publicos)
                dineroExtra, // Cantidad de dinero que se agrega
                ItemClass.Service.None,
                ItemClass.SubService.None,
                ItemClass.Level.None);
        }
    }
}