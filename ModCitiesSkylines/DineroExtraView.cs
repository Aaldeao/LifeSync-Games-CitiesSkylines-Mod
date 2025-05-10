using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using ColossalFramework.UI;
using UnityEngine;
using System.Collections;
using ColossalFramework.Plugins;

namespace ModCitiesSkylines
{
    public static class DineroExtraView
    {
        // Método para mostrar mensajes personalizado
        public static void MostrarMensajeDinero(string titulo, string mensaje, string dineroIcono)
        {
            UIView view = UIView.GetAView();

            // Crear panel principal
            UIPanel panel = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.backgroundSprite = "MenuPanel2";
            panel.opacity = 0f;
            panel.size = new Vector2(510f, 140f); 
            panel.relativePosition = new Vector3((view.fixedWidth - panel.width) / 2, (view.fixedHeight - panel.height) / 2);
            panel.isVisible = true;
            panel.clipChildren = true;

            // Título 
            UILabel tituloLabel = panel.AddUIComponent<UILabel>();
            tituloLabel.text = titulo;
            tituloLabel.textScale = 1.4f;
            tituloLabel.textColor = new Color32(255, 255, 255, 255);
            tituloLabel.autoSize = true;
            tituloLabel.relativePosition = new Vector3((panel.width - tituloLabel.width) / 2, 10f);


            float iconT = 72f;
            float espacio = 15f;

            // Cargar textura e insertar icono 
            Texture2D texture = LoadTexture(dineroIcono);
            if (texture != null)
            {
                UITextureSprite icono = panel.AddUIComponent<UITextureSprite>();
                icono.texture = texture;
                icono.size = new Vector2(iconT, iconT);
                icono.relativePosition = new Vector3(espacio, (panel.height - iconT) / 2 + 20f);
            }


            // Mensaje
            UILabel mensajeLabel = panel.AddUIComponent<UILabel>();
            mensajeLabel.text = mensaje;
            mensajeLabel.textScale = 1.0f;
            mensajeLabel.textColor = new Color32(230, 230, 230, 255);
            mensajeLabel.autoSize = true;
            mensajeLabel.relativePosition = new Vector3(iconT + 2 * espacio, 60f);


            // Fade-in
            panel.StartCoroutine(FadeIn(panel, 0.2f));

            // Fade-out
            panel.StartCoroutine(CerrarMensaje(panel, 4f));
        }


        // Cargar el icono desde los recursos embebidos
        private static Texture2D LoadTexture(string icono)
        {
            var assembly = typeof(DineroExtraView).Assembly;
            using (var stream = assembly.GetManifestResourceStream(icono))
            {
                if (stream == null)
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "No se encontró el icono: " + icono);
                    return null;
                }

                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(data);
                return texture;
            }
        }


        // Animación de desaparición
        private static IEnumerator CerrarMensaje(UIPanel panel, float segundos)
        {
            yield return new WaitForSeconds(segundos);
            yield return panel.StartCoroutine(FadeOut(panel, 0.5f));
        }

        // Aparición del panel con efecto de desvanecimiento (fade-in)
        private static IEnumerator FadeIn(UIPanel panel, float duracion)
        {
            float t = 0;
            while (t < duracion)
            {
                panel.opacity = Mathf.Lerp(0f, 0.95f, t / duracion);
                t += Time.deltaTime;
                yield return null;
            }
            panel.opacity = 0.95f;
        }

        // Desaparición del panel con efecto de desvanecimiento (fade-out)
        private static IEnumerator FadeOut(UIPanel panel, float duracion)
        {
            float t = 0;
            float inicio = panel.opacity;
            while (t < duracion)
            {
                panel.opacity = Mathf.Lerp(inicio, 0f, t / duracion);
                t += Time.deltaTime;
                yield return null;
            }
            panel.opacity = 0f;
            GameObject.Destroy(panel.gameObject);
        }
    }
}