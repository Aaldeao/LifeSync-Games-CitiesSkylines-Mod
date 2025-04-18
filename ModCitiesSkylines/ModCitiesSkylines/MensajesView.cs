using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using ColossalFramework.UI;
using UnityEngine;
using System.Collections;

namespace ModCitiesSkylines
{
    public static class MensajesView
    {
        // Método para mostrar mensajes personalizado
        public static void MostrarMensajeDinero(string titulo, string mensaje, string dineroIcono)
        {
            UIView view = UIView.GetAView();

            //Configuración del panel
            UIPanel panel = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.backgroundSprite = "InfoDisplay";
            panel.opacity = 0.95f;
            panel.size = new Vector2(600f, 200f);
            panel.relativePosition = new Vector3((view.fixedWidth - panel.width) / 2, (view.fixedHeight - panel.height) / 2);
            panel.isVisible = true;
            panel.clipChildren = true;

            float espacio = 20f; //espacio entre el icono y el texto
            float iconT = 128f; //tamaño del icono

            Texture2D texture = LoadTexture(dineroIcono);
            if (texture != null)
            {
                UITextureSprite icono = panel.AddUIComponent<UITextureSprite>();
                icono.texture = texture;
                icono.size = new Vector2(iconT, iconT);
                icono.relativePosition = new Vector3(espacio, (panel.height - iconT) / 2);
            }

            // Configuración del título
            UILabel tituloLabel = panel.AddUIComponent<UILabel>();
            tituloLabel.text = titulo;
            tituloLabel.textScale = 1.4f; // tamaño del texto
            tituloLabel.textColor = new Color32(255, 255, 255, 255);
            tituloLabel.relativePosition = new Vector3(iconT + 2 * espacio, 30);
            tituloLabel.autoSize = true;

            // Configuración del mensaje
            UILabel mensajeLabel = panel.AddUIComponent<UILabel>();
            mensajeLabel.text = mensaje;
            mensajeLabel.textScale = 0.8f; // tamaño del texto
            mensajeLabel.textColor = new Color32(240, 240, 240, 255);
            mensajeLabel.relativePosition = new Vector3(iconT + 2 * espacio, 70);
            mensajeLabel.autoSize = true;

            panel.StartCoroutine(CerrarMensaje(panel, 4f)); // tiempo en segundos para cerrar el mensaje


        }

        // Método para cargar el icono y mostrarlo en el mensaje
        private static Texture2D LoadTexture(string icono)
        {
            var assembly = typeof(MensajesView).Assembly;
            using (var stream = assembly.GetManifestResourceStream(icono))
            {
                if (stream == null)
                {
                    Debug.LogError("No se encontró el recurso embebido: " + icono);
                    return null;
                }

                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(data);
                return texture;
            }
        }

        private static IEnumerator CerrarMensaje(UIPanel panel, float segundos)
        {
            yield return new WaitForSeconds(segundos);
            GameObject.Destroy(panel.gameObject);
        }

    }
}