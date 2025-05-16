using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace ModCitiesSkylines
{
    public class BotonbG : LoadingExtensionBase
    {
        private UIButton boton;

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            // Verifica si el modo de carga es Nuevo Juego o Cargar Juego
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame)
            {
                BotonbGames(); // LLama al método para crear el botón
            }
        }

        public void BotonbGames()
        {
            var uiView = UIView.GetAView();

            // Cargar el icono
            Texture2D texture = LoadTexture("ModCitiesSkylines.ITO-iso.png");
            if (texture == null)
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, "No se pudo cargar el icono.");
                return;
            }

            // Crear atlas a partir de la textura(icono)
            UITextureAtlas atlas = CreateAtlas(texture, "MiButonA", "MiButon");

            // Crear botón a la vista
            boton = uiView.AddUIComponent(typeof(UIButton)) as UIButton;
            boton.name = "MiButon";

            boton.atlas = UIView.GetAView().defaultAtlas; // Usa el atlas del icono por defecto

            // Estilo del botón
            boton.normalBgSprite = "ButtonMenu";
            boton.hoveredBgSprite = "ButtonMenuHovered";
            boton.pressedBgSprite = "ButtonMenuPressed";
            boton.disabledBgSprite = "ButtonMenuDisabled";

            boton.size = new Vector2(48, 48); // Tamaño del botón
            boton.isInteractive = true;

            // Posicion del boton debajo del botón de opciones
            var botonAjuste = uiView.FindUIComponent<UIButton>("Esc");
            if (botonAjuste != null)
            {
                Vector3 posicion = botonAjuste.absolutePosition;
                boton.relativePosition = new Vector3(posicion.x - 1, posicion.y + botonAjuste.size.y + 10);
            }

            // Agregar icono encima del boton
            var icono = boton.AddUIComponent<UISprite>();
            icono.atlas = atlas;
            icono.spriteName = "MiButon";
            icono.size = new Vector2(55f, 40f); // Tamaño del icono
            icono.relativePosition = new Vector2((boton.width - icono.width) / 2, (boton.height - icono.height) / 2);

            // Acción al hacer clic
            boton.eventClick += (component, eventParam) =>
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "¡Botón presionado!");
                if (LoginPanel.inicioSesion)
                {
                    // Si el usuario ya ha iniciado sesión, muestra el perfil
                    PuntosJG.ObtenerPuntos();
                }
                else
                {
                    // Si no ha iniciado sesión, muestra el panel del login
                    LoginPanel.mostrarLogin();
                }
            };
        }

        // Cargar la textura desde los recursos incrustados
        private static Texture2D LoadTexture(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // Obtiene el DLL del mod

            // Abre el recurso por nombre (la imagen PNG)
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, $"No se pudo encontrar el recurso: {resourceName}");
                    return null;
                }

                // Lee el archivo completo como bytes
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                // Crea la textura desde los datos cargados
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(buffer);
                return texture;
            }
        }

        // Crear un atlas (Conjunto de texturas) a partir del icono el cual nos permite usarlo en el botón
        private static UITextureAtlas CreateAtlas(Texture2D texture, string atlasName, string spriteName)
        {
            // Crea un nuevo objeto tipo UITextureAtlas
            UITextureAtlas atlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            atlas.name = atlasName; // Asigna el nombre del atlas

            // Crea un material para el atlas y le asigna la textura
            Material material = new Material(Shader.Find("UI/Default UI Shader"));
            material.mainTexture = texture;
            atlas.material = material;

            // Define la información del sprite (usa toda la imagen como región)
            var spriteInfo = new UITextureAtlas.SpriteInfo
            {
                name = spriteName,
                texture = texture,
                region = new Rect(0f, 0f, 1f, 1f) // Ocupa toda la textura
            };

            // Agrega el sprite al atlas
            atlas.AddSprite(spriteInfo);

            return atlas;
        }
    }
}
