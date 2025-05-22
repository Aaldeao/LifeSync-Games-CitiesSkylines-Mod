using System;
using System.Collections.Generic;
using UnityEngine;
using ColossalFramework.UI;
using System.Linq;
using System.Security.Cryptography;

using bGamesAPI;

namespace ModCitiesSkylines
{
    public class CanjePuntosPanel
    {
        private static UIPanel panelCanje;

        private static Dictionary<string, UITextField> camposPorAtributo;
        private static UIButton closeBtn;
        private static UIButton btnConfirmar;

        // Método para mostrar el panel de canje de puntos
        public static void CanjePanel(List<PerfilJGView.AtributoUsuario> atributos, int totalDisponibles)
        {
            UIView view = UIView.GetAView();
            if (panelCanje != null)
                GameObject.Destroy(panelCanje.gameObject);

            // Panel del canje de puntos
            panelCanje = view.AddUIComponent(typeof(UIPanel)) as UIPanel;
            panelCanje.backgroundSprite = "MenuPanel2";
            panelCanje.size = new Vector2(400f, 350f);
            panelCanje.relativePosition = new Vector2((view.fixedWidth - panelCanje.width) / 2f, (view.fixedHeight - panelCanje.height) / 2f);

            // Título del panel
            UILabel tituloLabel = panelCanje.AddUIComponent<UILabel>();
            tituloLabel.text = "LifeSync Games";
            tituloLabel.textScale = 1.6f; // Tamaño del texto
            tituloLabel.textColor = new Color32(255, 255, 255, 255);
            tituloLabel.relativePosition = new Vector2((panelCanje.width - tituloLabel.width) / 2f, 8f);

            // Subtítulo del panel
            UILabel subtitulo = panelCanje.AddUIComponent<UILabel>();
            subtitulo.text = "Canjea tu puntos por ... :";
            subtitulo.textScale = 1.0f;
            subtitulo.textColor = new Color32(255, 255, 255, 255);
            subtitulo.relativePosition = new Vector2((panelCanje.width - subtitulo.width) / 2f, 55f);

            // Campo de texto para ingresar cantidad de puntos a canjear por dimensión
            camposPorAtributo = new Dictionary<string, UITextField>();

            float y = 100f;
            foreach (var atributo in atributos)
            {
                UILabel lbl = panelCanje.AddUIComponent<UILabel>();
                lbl.text = $"{atributo.Atributo} ({atributo.Punto}):";
                lbl.relativePosition = new Vector2(20f, y);

                // Boton menos
                UIButton btnMenos = panelCanje.AddUIComponent<UIButton>();
                btnMenos.text = "-";
                btnMenos.size = new Vector2(28f, 25f);
                btnMenos.relativePosition = new Vector2(250f, y - 5f);
                EstiloBtn(btnMenos);

                UITextField input = panelCanje.AddUIComponent<UITextField>();
                input.size = new Vector2(50f, 25f);
                input.relativePosition = new Vector2(280f, y - 5f);
                input.numericalOnly = true;
                input.text = "0";
                input.readOnly = true;
                input.isInteractive = false;
                input.textColor = new Color32(0, 0, 0, 255);
                input.selectionSprite = "EmptySprite";
                input.normalBgSprite = "TextFieldPanel";
                input.padding = new RectOffset(6, 6, 6, 6);

                // Boton más
                UIButton btnMas = panelCanje.AddUIComponent<UIButton>();
                btnMas.text = "+";
                btnMas.size = new Vector2(28f, 25f);
                btnMas.relativePosition = new Vector2(335f, y - 5f);
                EstiloBtn(btnMas);

                // Evento para manejar el clic en el botón "más" y "menos"

                btnMas.eventClick += (component, param) =>
                {
                    // Convierte el texto a un número entero y lo guarda en la variable actual
                    int actual = int.Parse(input.text); // que es un 0 ese valor

                    // Verififica que el número actual(0) sea menor que el máximo permitido
                    if (actual < atributo.Punto)
                    {
                        // Si es menor, incrementa el valor
                        actual++;
                        input.text = actual.ToString(); // Actualiza el campo de texto
                    }
                };
                btnMenos.eventClick += (component, param) =>
                {
                    int actual = int.Parse(input.text);
                    // Verifica que el número actual sea mayor que cero
                    if (actual > 0)
                    {
                        // Si es mayor, disminuye el valor
                        actual--;
                        input.text = actual.ToString();
                    }
                };

                // Almacena los valores de los puntos que puso que queria canjear por dimension
                camposPorAtributo[atributo.Atributo] = input;
                y += 35f;
            }

            // Botón de cerrar el panel
            closeBtn = panelCanje.AddUIComponent<UIButton>();
            closeBtn.size = new Vector2(30f, 30f);
            closeBtn.relativePosition = new Vector2(panelCanje.width - 40f, 5f);
            closeBtn.normalBgSprite = "buttonclose";
            closeBtn.hoveredBgSprite = "buttonclosehover";
            closeBtn.pressedBgSprite = "buttonclosepressed";
            closeBtn.eventClick += (component, param) =>
            {
                panelCanje.isVisible = false;  // Oculta el panel
            };

            // Botón para confirmar los puntos que desea canjear
            btnConfirmar = panelCanje.AddUIComponent<UIButton>();
            btnConfirmar.text = "Confirmar";
            btnConfirmar.size = new Vector2(150f, 30f);
            EstiloBtn(btnConfirmar);
            btnConfirmar.relativePosition = new Vector2((panelCanje.width - btnConfirmar.width) / 2f, y + 20f);
            btnConfirmar.eventClick += (component, eventParam) =>
            {
                // Calcular el total de puntos que el usuario escribio en los campos de cada dimensión
                int totalPuntos = camposPorAtributo.Values.Sum(field =>
                {
                    int val;
                    return int.TryParse(field.text, out val) ? val : 0;
                });

                // Valida que el total de los puntos no sea cero
                if (totalPuntos == 0)
                {
                    MsgView.PanelMSG("LifeSync Games", "Debes colocar puntos en tus dimensiones para poder canjear.");
                    return;
                }

                // Mostrar un mensaje de confirmación antes de canjear los puntos
                MostrarConfirmacionCanje(
                    "LifeSync Games",
                    $"¿Estás seguro de que deseas canjear {totalPuntos} {(totalPuntos == 1 ? "punto" : "puntos")}? ",
                    () => ConfirmarCanje(atributos, totalDisponibles)
                );
            };
        }

        // Estilo del botón
        private static void EstiloBtn(UIButton boton)
        {
            boton.normalBgSprite = "ButtonMenu";
            boton.hoveredBgSprite = "ButtonMenuHovered";
            boton.pressedBgSprite = "ButtonMenuPressed";
            boton.textColor = new Color32(255, 255, 255, 255);
            boton.hoveredTextColor = new Color32(200, 200, 200, 255);
            boton.pressedTextColor = new Color32(150, 150, 150, 255);
        }

        private static void ConfirmarCanje(List<PerfilJGView.AtributoUsuario> atributos, int totalDisponibles)
        {
            int totalcanjear = 0;

            // Recolectar errores y puntos asignados
            List<string> atributosExcedidos = new List<string>(); // Lista para almacenar atributos excedidos
            Dictionary<string, int> puntosAsignados = new Dictionary<string, int>(); // Almacena los puntos asignados por atributo

            int sumaDePuntos = 0; // La suma de los puntos a canjear

            // Recorrer los atributos y verificar los puntos asignados
            foreach (var atributo in atributos)
            {
                int val = 0;
                int.TryParse(camposPorAtributo[atributo.Atributo].text, out val);

                // Verifica que no canjee más de lo que tiene
                if (val > atributo.Punto)
                {
                    atributosExcedidos.Add(atributo.Atributo);
                }

                puntosAsignados[atributo.Atributo] = val; // Almacena los puntos asignados por atributo
                sumaDePuntos += val; // Suma los puntos asignados
            }

            totalcanjear = sumaDePuntos; // Total de puntos a canjear es la suma de los puntos por dimensión

            // Muentra las dimensiones que exceden de su maximo
            if (atributosExcedidos.Count > 0)
            {
                string mensajeError = "Colocaste más de lo que tienes disponible en las estas dimensiones: " + string.Join(", ", atributosExcedidos.ToArray());
                MsgView.PanelMSG("LifeSync Games", mensajeError);
                return;
            }

            // Verificar si la suma de los puntos a canjear es mayor al total disponible
            if (totalcanjear > totalDisponibles)
            {
                MsgView.PanelMSG("LifeSync Games", $"La cantidad de puntos total ({totalcanjear}) que deseas por canjear es mayor al total disponible ({totalDisponibles}).");
                return;
            }

            // Realiza el canjeo de puntos
            foreach (var atributo in atributos)
            {

                
                if (!puntosAsignados.TryGetValue(atributo.Atributo, out int cantidad) || cantidad == 0)
                    continue;   // Si no hay cantidad asignada, salta al siguiente atributo

                int nuevoValor = atributo.Punto - cantidad;

                // Envía lo canjeado a la API
                var respuesta = CanjearPts.EnviarCanje(
                    LoginPanel.idJugador.Value,     // id_player
                    atributo.IdAtributo,            // id_attributes
                    nuevoValor                      // data (nuevo valor de puntos)
                );

                if (!respuesta.Exito)
                {
                    MsgView.PanelMSG(respuesta.Titulo, respuesta.Mensaje);
                    return;   // aborta todo el proceso si falla un atributo
                }
            }

            // Cerrar el panel
            GameObject.Destroy(panelCanje.gameObject);
            panelCanje = null;

            // Mensaje de éxito
            MsgView.PanelMSG("LifeSync Games", $"Has canjeado {sumaDePuntos} {(sumaDePuntos == 1 ? "punto" : "puntos")}");

            // Actualizar los puntos del jugador
            PuntosJG.ObtenerPuntos();

        }

        // Este muestra el diálogo de confirmación con botones "Sí / No"
        private static void MostrarConfirmacionCanje(string titulo, string mensaje, Action onAceptar)
        {
            ConfirmPanel.ShowModal(titulo, mensaje, (component, ret) =>
            {
                if (ret == 1) // "Sí" fue presionado
                {
                    onAceptar?.Invoke(); // Llama a la acción de confirmación
                }
                // Si presionan "No", no hacemos nada
            });
        }

        // Método para cerrar el panel para canjear puntos
        public static void CerrarPanel()
        {
            if (panelCanje != null)
            {
                GameObject.Destroy(panelCanje.gameObject);
                panelCanje = null;
            }
        }

        // Método para verificar si el panel para canjear está abierto
        public static bool EstaAbierto()
        {
            return panelCanje != null && panelCanje.isVisible;
        }
    }
}

