using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class Login : ThreadingExtensionBase
    {
        // Tecla para mostrar el panel de login
        private const KeyCode Login_Tecla = KeyCode.F2;

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (Input.GetKeyDown(Login_Tecla))
            {
                LoginPanel.mostrarLogin();
            }
        }
    }
}

