using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ICities;
using System.IO;
using System.Net;
using UnityEngine;
using ColossalFramework.UI;
using ColossalFramework.Plugins;
using bGamesAPI;


namespace ModCitiesSkylines
{
    public class LoginView : LoadingExtensionBase
    {
        // Método que se llama al cargar el juego
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            LoginPanel.CrearPanel(); // Llama al método que construye el panel del login
        }
    }

    public static class LoginPanel
    {
        private static UIPanel panelLogin;
        private static UITextField usuario;
        private static UITextField password;
        private static UIButton loginButton;
        private static UIButton closeButton;
        private static UIButton closeXButton;

        public static int? idJugador = null; // Donde se almacenará el ID del jugador

        public static void CrearPanel()
        {
            var view = UIView.GetAView();
            if (view == null)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "No se obtuvo el login");
                return;
            }
            // Verifica si el panel ya existe
            if (panelLogin != null) return;

            // Crear el panel de login
            panelLogin = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panelLogin.backgroundSprite = "InfoDisplay";
            panelLogin.opacity = 0.95f;
            panelLogin.size = new Vector2(600f, 280f); // Tamaño del panel
            panelLogin.relativePosition = new Vector2((view.fixedWidth - panelLogin.width) / 2, (view.fixedHeight - panelLogin.height) / 2);
            panelLogin.isInteractive = true; // Permitir la interacción con el panel
            panelLogin.canFocus = true;
            panelLogin.isEnabled = true;
            panelLogin.isVisible = false;

            // Título
            UILabel tituloLabel = panelLogin.AddUIComponent<UILabel>();
            tituloLabel.text = "LifeSync Games";
            tituloLabel.textScale = 1.6f; // Tamaño del texto
            tituloLabel.textColor = new Color32(255, 255, 255, 255);
            tituloLabel.relativePosition = new Vector2((panelLogin.width - tituloLabel.width) / 2f, 25f);

            // Cargar textura e insertar icono
            Texture2D texture = LoadTexture("ModCitiesSkylines.ITO-iso.png");
            if (texture != null)
            {
                UITextureSprite logoSprite = panelLogin.AddUIComponent<UITextureSprite>();
                logoSprite.texture = texture;
                logoSprite.size = new Vector2(75f, 60f); // Ajustar el tamaño del logo
                logoSprite.relativePosition = new Vector2(20f, 15f); // Posición izquierda y vertical centrada con el título
            }

            // Titulo del campo de texto para el correo
            UILabel tituloLabelCorreo = panelLogin.AddUIComponent<UILabel>();
            tituloLabelCorreo.text = "Correo:";
            tituloLabelCorreo.relativePosition = new Vector2(20f, 85f); // Ajusta el título del correo

            // Campo de texto para el correo
            usuario = panelLogin.AddUIComponent<UITextField>();
            usuario.relativePosition = new Vector2(20f, 105f); // Ajusta la posición del campo de texto
            usuario.size = new Vector2(560f, 30f);
            usuario.text = "";
            usuario.isInteractive = true;
            usuario.readOnly = false;
            usuario.canFocus = true;
            usuario.enabled = true;
            usuario.builtinKeyNavigation = true; // Permitir la navegación por teclado
            usuario.selectionSprite = "EmptySprite";
            usuario.normalBgSprite = "TextFieldPanel";
            usuario.hoveredBgSprite = "TextFieldPanelHovered";
            usuario.focusedBgSprite = "TextFieldPanelFocused";
            usuario.textColor = new Color32(0, 0, 0, 255);
            usuario.padding = new RectOffset(5, 5, 5, 5);

            // Titulo del campo de texto para la contraseña
            UILabel tituloLabelContrasena = panelLogin.AddUIComponent<UILabel>();
            tituloLabelContrasena.text = "Contraseña:";
            tituloLabelContrasena.relativePosition = new Vector2(20f, 150f); // Ajusta el título de la contraseña

            // Campo de texto para la contraseña
            password = panelLogin.AddUIComponent<UITextField>();
            password.relativePosition = new Vector2(20f, 170f);  // Ajusta la posición del campo de texto
            password.size = new Vector2(560f, 30f);
            password.text = "";
            password.isPasswordField = true;
            password.isInteractive = true;
            password.readOnly = false;
            password.canFocus = true;
            password.enabled = true;
            password.builtinKeyNavigation = true;
            password.selectionSprite = "EmptySprite";
            password.normalBgSprite = "TextFieldPanel";
            password.hoveredBgSprite = "TextFieldPanelHovered";
            password.focusedBgSprite = "TextFieldPanelFocused";
            password.textColor = new Color32(0, 0, 0, 255);
            password.padding = new RectOffset(5, 5, 5, 5);

            // Botón de iniciar sesión
            loginButton = panelLogin.AddUIComponent<UIButton>();
            loginButton.text = "Iniciar Sesión";
            loginButton.relativePosition = new Vector2((panelLogin.width - 270) / 2, 225f); // Ajusta la posición del botón
            loginButton.size = new Vector2(270f, 30f);
            EstiloBotones(loginButton);
            loginButton.eventClick += (component, eventParam) =>
            {
                IniciarSesion();
            };

            // Botón de cerrar sesión
            //closeButton = panelLogin.AddUIComponent<UIButton>();
            //closeButton.text = "Cerrar Sesión";
            //closeButton.relativePosition = new Vector2(310f, 200f);
            //closeButton.size = new Vector2(270f, 30f);
            //EstiloBotones(closeButton);
            //closeButton.eventClick += (component, eventParam) =>
            //{
                //CerrarSesion();
                //CerrarLogin();
            //};

            // Botón de cerrar el panel
            closeXButton = panelLogin.AddUIComponent<UIButton>();
            closeXButton.text = "X";
            closeXButton.size = new Vector2(30f, 30f);
            EstiloBotones(closeXButton);
            closeXButton.relativePosition = new Vector2(panelLogin.width - 35f, 5f);
            closeXButton.eventClick += (component, eventParam) =>
            {
                CerrarLogin();
            };


            // Enter para el campo de usuario y contraseña
            usuario.eventKeyDown += (component, eventParam) => Enter(component, eventParam);
            password.eventKeyDown += (component, eventParam) => Enter(component, eventParam);
        }

        // Método para mostrar el panel de login
        public static void mostrarLogin()
        {
            if (panelLogin != null)
            {
                panelLogin.isVisible = !panelLogin.isVisible;
                if (panelLogin.isVisible)
                {
                    panelLogin.isInteractive = true;
                    panelLogin.isEnabled = true;
                }
            }
        }

        private static void EstiloBotones(UIButton boton)
        {
            boton.normalBgSprite = "ButtonMenu";
            boton.hoveredBgSprite = "ButtonMenuHovered";
            boton.pressedBgSprite = "ButtonMenuPressed";
            boton.disabledBgSprite = "ButtonMenuDisabled";
            boton.textColor = new Color32(255, 255, 255, 255);
            boton.hoveredTextColor = new Color32(200, 200, 200, 255);
            boton.pressedTextColor = new Color32(150, 150, 150, 255);
        }

        private static void IniciarSesion()
        {
            var correo = usuario.text.Trim();
            var contrasena = password.text.Trim();

            // Valida que los campos no estén vacíos
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
            {
                MostrarMensaje("Campos incompletos", "Por favor complete los campos vacios.");
                return;
            }

            var resultado = AutenticacionJG.IniciarSesion(correo, contrasena);

            if (resultado.IdJugador.HasValue) 
            {
                idJugador = resultado.IdJugador.Value;
                MostrarMensaje("Inicio de sesión exitoso", "Bienvenido a LifeSync Games");
                panelLogin.Hide();
            }
            else
            {
                MostrarMensaje(resultado.Titulo, resultado.Mensaje);
            }
        }

        // Método para cerrar sesión y limpiar los campos
        private static void CerrarSesion()
        {
            idJugador = null;
            MostrarMensaje("Cerrar sesión", "Has cerrado tu sesión correctamente.");
            if (usuario != null) usuario.text = "";
            if (password != null) password.text = "";
            panelLogin.Hide();
        }

        // Método para cerrar el panel de login
        private static void CerrarLogin()
        {
            mostrarLogin();
        }

        private static void MostrarMensaje(string titulo, string mensaje)
        {
            UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel").SetMessage(titulo, mensaje, false);
        }

        // Método para apretar la tecla ENTER
        private static void Enter(UIComponent component, UIKeyEventParameter eventParam)
        {
            if (eventParam.used) return;

            if (eventParam.keycode == KeyCode.Return || eventParam.keycode == KeyCode.KeypadEnter)
            {
                IniciarSesion();
                eventParam.Use();
            }
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
    }
}