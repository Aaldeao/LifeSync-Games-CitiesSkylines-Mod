using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ModCitiesSkylines;
using System.Text.RegularExpressions;

namespace bGamesAPI
{
    public class AtributosJugador
    {
        public string Nombre { get; set; } // Para almacenar el nombre del atributo
        public int? Punto { get; set; } // Para almacenar el valor del atributo
    }
    public class ResultadosAtributos
    {
        public string Titulo { get; set; } // Para almacenar el título del resultado
        public string Mensaje { get; set; } // Para almacenar el mensaje del resultado

        public int? Puntos { get; set; } // Para almacenar los puntos del jugador
        public List<AtributosJugador> Atributos { get; set; } = new List<AtributosJugador>(); // Para almacenar los atributos del jugador

    }

    public class AtributosJG
    {
        public static ResultadosAtributos ObtenerPuntosdelJugador()
        {
            var resultado = new ResultadosAtributos();

            try
            {
                // Verificar si el jugador ha iniciado sesión
                if (!LoginPanel.idJugador.HasValue) // si el idJugador es null es porque no se ha iniciado sesión
                {
                    resultado.Titulo = "Puntos de LifeSync Games";
                    resultado.Mensaje = "No se ha iniciado sesión.";
                    return resultado;
                }

                string url = $"http://localhost:3001/player_all_attributes/{LoginPanel.idJugador}"; // URL de la API de bGames
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // Leer la respuesta de la API
                    string result = reader.ReadToEnd();

                    var name = Regex.Matches(result, @"""name""\s*:\s*""(.*?)"""); // Expresión regular para extraer el nombre del atributo
                    var datos = Regex.Matches(result, @"""data""\s*:\s*(\d+)"); // Expresión regular para extraer los datos buscando el valor de "data"
                    
                    int sumaPuntos = 0;
                    if (name.Count == datos.Count && datos.Count>0)
                    {
                        for (int i = 0; i < datos.Count; i++)
                        {
                            string nombre = name[i].Groups[1].Value; // Obtener el nombre del atributo
                            int punto = int.Parse(datos[i].Groups[1].Value); // Obtener el valor del atributo
                            sumaPuntos += punto;
                            resultado.Atributos.Add(new AtributosJugador { Nombre = nombre, Punto = punto }); // Agregar el atributo a la lista

                        }
                        resultado.Titulo = "Puntos de LifeSync Games";
                        resultado.Puntos = sumaPuntos;
                        resultado.Mensaje = sumaPuntos == 1
                            ? $"Tienes {sumaPuntos} punto en LifeSync Games."
                            : $"Tienes {sumaPuntos} puntos en LifeSync Games.";

                    }
                    else
                    {
                        resultado.Titulo = "Sin puntos en LifeSync Games";
                        resultado.Mensaje = "No cuentas con puntos en tu perfil de LifeSync Games.";
                    }

                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción si la conexión falla
                resultado.Titulo = "Error de conexión";
                resultado.Mensaje = "Error de conexión: " + ex.Message;

            }
            return resultado; // Devolver el resultado de la conexión
        }
    }
}

