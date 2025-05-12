using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.UI;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class ConexionView
    {
        public static void PanelConexio(string titulo, string mensaje)
        {
            UIView view = UIView.GetAView();
            // Panel principal
            UIPanel panel = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.name = "PanelConexion";
            panel.backgroundSprite = "MenuPanel2";
            panel.size = new Vector2(420f, 175f); // Tamaño del panel
            panel.relativePosition = new Vector3((view.fixedWidth - panel.width) / 2, (view.fixedHeight - panel.height) / 2);
            panel.isVisible = true;
            panel.canFocus = true;
            panel.isInteractive = true;

            // Título
            UILabel titleLabel = panel.AddUIComponent<UILabel>();
            titleLabel.text = titulo;
            titleLabel.textScale = 1.5f; // Tamaño del texto
            titleLabel.textAlignment = UIHorizontalAlignment.Center;
            titleLabel.autoSize = false;
            titleLabel.size = new Vector2(panel.width, 30f);
            titleLabel.relativePosition = new Vector3(0, 8); // La posición del título

            // Mensaje
            UILabel messageLabel = panel.AddUIComponent<UILabel>();
            messageLabel.text = mensaje;
            messageLabel.textScale = 1.2f; // Tamaño del texto
            messageLabel.wordWrap = true;
            messageLabel.autoSize = false;
            messageLabel.size = new Vector2(panel.width - 40, 80f);
            messageLabel.relativePosition = new Vector3(20f, 70f);
            messageLabel.textAlignment = UIHorizontalAlignment.Center;

            // Botón cerrar
            UIButton closeButton = panel.AddUIComponent<UIButton>();
            closeButton.text = "Cerrar";
            closeButton.width = 100f;
            closeButton.height = 30f;
            closeButton.relativePosition = new Vector3((panel.width - closeButton.width) / 2, 140f);
            closeButton.normalBgSprite = "ButtonMenu";
            closeButton.hoveredBgSprite = "ButtonMenuHovered";
            closeButton.pressedBgSprite = "ButtonMenuPressed";
            closeButton.textScale = 0.9f;
            closeButton.eventClick += (component, param) =>
            {
                GameObject.Destroy(panel.gameObject); // Destruir el panel al cerrar
            };
        }
    }
}
