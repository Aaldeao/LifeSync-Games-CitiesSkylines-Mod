using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ColossalFramework.UI;
using UnityEngine;
using ColossalFramework.Plugins;


namespace ModCitiesSkylines
{
    public class PerfilJGView
    {
        // Clase interna para representar los atributos del usuario
        public class AtributoUsuario
        {
            public string Atributo { get; set; }
            public int Punto { get; set; }
        }

        private static UIPanel panelPerfil;
        private static UILabel userLabel;
        private static UIButton closeBtn;
        private static UILabel totalP;
        private static UIButton closeButton;

        public static void PerfilPanel(string nombreUsuario, int totalPuntos, List<AtributoUsuario> atributos)
        {
            UIView view = UIView.GetAView();
            if (view == null) return;

            if (panelPerfil != null)
                GameObject.Destroy(panelPerfil.gameObject);

            // Crea el panel principal
            panelPerfil = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panelPerfil.name = "PanelPerfilUsuario";
            panelPerfil.backgroundSprite = "MenuPanel2";
            panelPerfil.opacity = 0.97f;
            panelPerfil.size = new Vector2(700f, 300f);
            panelPerfil.relativePosition = new Vector2((view.fixedWidth - panelPerfil.width) / 2, (view.fixedHeight - panelPerfil.height) / 2);
            panelPerfil.isInteractive = true;
            panelPerfil.canFocus = true;

            // Título
            UILabel tituloLabel = panelPerfil.AddUIComponent<UILabel>();
            tituloLabel.text = "LifeSync Games";
            tituloLabel.textScale = 1.6f; // Tamaño del texto
            tituloLabel.textColor = new Color32(255, 255, 255, 255);
            tituloLabel.relativePosition = new Vector2((panelPerfil.width - tituloLabel.width) / 2f, 8f);

            // Cargar avatar 
            Texture2D texture = LoadTexture("ModCitiesSkylines.usuario.png");
            if (texture != null)
            {
                UITextureSprite avatar = panelPerfil.AddUIComponent<UITextureSprite>();
                avatar.texture = texture;
                avatar.size = new Vector2(55f, 55f);// Ajustar el tamaño del logo
                avatar.relativePosition = new Vector2(20f, 50f);// Posición izquierda y vertical centrada con el título
            }

            // Nombre de usuario
            userLabel = panelPerfil.AddUIComponent<UILabel>();
            userLabel.text = nombreUsuario;
            userLabel.textScale = 1.2f;
            userLabel.relativePosition = new Vector2(90f, 65f);

            // Botón de cerrar el panel
            closeBtn = panelPerfil.AddUIComponent<UIButton>();
            closeBtn.size = new Vector2(30f, 30f);
            closeBtn.relativePosition = new Vector2(panelPerfil.width - 40f, 5f);
            closeBtn.normalBgSprite = "buttonclose";
            closeBtn.hoveredBgSprite = "buttonclosehover";
            closeBtn.pressedBgSprite = "buttonclosepressed";
            closeBtn.eventClick += (component, param) =>
            {
                panelPerfil.isVisible = false;  // Oculta el panel
            }; 

            // Botón de cerrar sesión
            closeButton = panelPerfil.AddUIComponent<UIButton>();
            closeButton.text = "Cerrar Sesión";
            closeButton.size = new Vector2(150f, 30f);
            closeButton.relativePosition = new Vector2(panelPerfil.width - closeButton.width - 10f, panelPerfil.height - closeButton.height - 10f);
            EstiloBtn(closeButton);
            closeButton.eventClick += (component, eventParam) =>
            {
                CerrarSesion();
            };

            // Tarjetas de atributos
            float anchoT = 100f;  // Ancho de cada tarjeta
            float altoT = 60f;  // Alto de cada tarjeta
            float espacioT = 20f;    // Espacio entre tarjetas
            int maxTarjetas = 5; // Número máximo de tarjetas por fila

            int count = 0; // Contador de tarjetas
            int fila = 0;  // Contador de filas

            for (int i = 0; i < atributos.Count; i++)
            {
                var atributo = atributos[i];

                int tarjetasRestantes = atributos.Count - (fila * maxTarjetas); // Verifica cuántas tarjetas quedan por mostrar
                int tarjetasEnFila = Math.Min(maxTarjetas, tarjetasRestantes);  // Verifica cuántas tarjetas se pueden mostrar en la fila actual

                float filaAncho = tarjetasEnFila * anchoT + (tarjetasEnFila - 1) * espacioT; // Ancho total de la fila
                float posXBase = (panelPerfil.width - filaAncho) / 2f; // Centra la fila de tarjetas

                // Calcular la posición de la tarjeta
                float posX = posXBase + count * (anchoT + espacioT);
                float posY = 120f + fila * (altoT + espacioT);

                // Crear tarjeta del atributo
                UIPanel atri = panelPerfil.AddUIComponent<UIPanel>();
                atri.backgroundSprite = "GenericPanel";
                atri.size = new Vector2(anchoT, altoT);
                atri.relativePosition = new Vector2(posX, posY);

                // Nombre del atributo 
                UILabel nombre = atri.AddUIComponent<UILabel>();
                nombre.text = atributo.Atributo;
                nombre.textScale = 0.9f;
                nombre.autoSize = true;
                nombre.relativePosition = new Vector2((anchoT - nombre.width) / 2f, 5f);

                // Valor
                UILabel valor = atri.AddUIComponent<UILabel>();
                valor.text = atributo.Punto.ToString();
                valor.textScale = 1.2f;
                valor.textColor = new Color32(0, 0, 0, 255);
                valor.autoSize = true;
                valor.relativePosition = new Vector2((anchoT - valor.width) / 2f, 30f);

                count++;

                // Si alcanzó el máximo por fila, Se genera una nueva fila
                if (count >= maxTarjetas)
                {
                    count = 0;
                    fila++;
                }
            }

            // Total de puntos 
            totalP = panelPerfil.AddUIComponent<UILabel>();
            totalP.text = "Total: " + totalPuntos + " puntos";
            totalP.textScale = 1.1f;
            totalP.autoSize = true;
            totalP.relativePosition = new Vector2((panelPerfil.width - totalP.width) / 2f, 205f);
        }
        private static Texture2D LoadTexture(string logo)
        {
            var assambly = typeof(LoginPanel).Assembly;
            using (var stream = assambly.GetManifestResourceStream(logo))
            {
                if (stream == null)
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, $"No se pudo cargar el icono: {logo}");
                    return null;
                }
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(data);
                return texture;
            }
        }
        private static void EstiloBtn(UIButton boton)
        {
            boton.normalBgSprite = "ButtonMenu";
            boton.hoveredBgSprite = "ButtonMenuHovered";
            boton.pressedBgSprite = "ButtonMenuPressed";
            boton.textColor = new Color32(255, 255, 255, 255);
            boton.hoveredTextColor = new Color32(200, 200, 200, 255);
            boton.pressedTextColor = new Color32(150, 150, 150, 255);
        }

        // Método para cerrar sesión, limpiar los campos del perfil y eliminar el panel 
        private static void CerrarSesion()
        {

            LoginPanel.idJugador = null;  // Restablece el ID del jugador a null
            if (LoginPanel.usuario != null) LoginPanel.usuario.text = "";  // Limpia el campo de texto del correo del usuario en el login
            if (LoginPanel.password != null) LoginPanel.password.text = "";  // Limpia el campo de texto de la contraseña en el login

            // Limpia los datos del perfil
            userLabel.text = "Usuario Desconocido";
            totalP.text = "Total: 0 puntos";

            // Elimina el panel de perfil
            GameObject.Destroy(panelPerfil.gameObject);
            panelPerfil = null;

            // Mostrar un mensaje si lo deseas
            MensajePerfil("Cerrar sesión", "Has cerrado sesión correctamente.");
        }
        private static void MensajePerfil(string titulo, string mensaje)
        {
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(titulo, mensaje, false);
        }

    }

}