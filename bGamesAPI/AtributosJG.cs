using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ModCitiesSkylines;
using System.Text.RegularExpressions;
using bGamesAPI;
using System.Xml.Linq;

namespace bGamesAPI
{
    public class AtributosJugador
    {
        public string Nombre { get; set; } // Para almacenar el nombre del atributo
        public int? Punto { get; set; } // Para almacenar el valor del atributo
        public int? Id { get; set; } // Para almacenar el id del atributo
    }
    public class ResultadosAtributos
    {
        public string Titulo { get; set; } // Para almacenar el título del resultado
        public string Mensaje { get; set; } // Para almacenar el mensaje del resultado

        public int? PuntosT { get; set; } // Para almacenar los puntos del jugador
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
                    resultado.Titulo = "LifeSync Games";
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

                    var ids = Regex.Matches(result, @"""id_attributes""\s*:\s*(\d+)"); // Expresión regular para extraer el id del atributo
                    var name = Regex.Matches(result, @"""name""\s*:\s*""(.*?)"""); // Expresión regular para extraer el nombre del atributo
                    var datos = Regex.Matches(result, @"""data""\s*:\s*(\d+)"); // Expresión regular para extraer los datos buscando el valor de "data"

                    int sumaPuntos = 0; // Variable para almacenar la suma total de los puntos

                    if (ids.Count == name.Count && name.Count == datos.Count && datos.Count > 0)
                    {
                        for (int i = 0; i < datos.Count; i++)
                        {
                            int id = int.Parse(ids[i].Groups[1].Value); // Obtener el id del atributo
                            string nombre = name[i].Groups[1].Value; // Obtener el nombre del atributo
                            int punto = int.Parse(datos[i].Groups[1].Value); // Obtener el valor del atributo
                            sumaPuntos += punto;

                            // Agregar el atributo a la lista de atributos con el id, nombre y punto 
                            resultado.Atributos.Add(new AtributosJugador
                            {
                                Id = id,               
                                Nombre = nombre,
                                Punto = punto
                            }); 
                        }

                        resultado.Titulo = "LifeSync Games";
                        resultado.PuntosT = sumaPuntos; // Almacenar la suma total de los puntos
                    }
                    else
                    {
                        resultado.Titulo = "LifeSync Games";
                        resultado.Mensaje = "No se pudieron leer correctamente los atributos del jugador.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Titulo = "Error de conexión";
                resultado.Mensaje = "Ocurrió un problema al intentar conectarse. Por favor, revisa tu conexión a internet o con la API de LifeSync Games.";
            }

            return resultado;
        }
    }
}

