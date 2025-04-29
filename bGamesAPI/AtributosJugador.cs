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
    public class ResultadosAtributos
    {
        public string Titulo { get; set; } // Para almacenar el título del resultado
        public string Mensaje { get; set; } // Para almacenar el mensaje del resultado

        public int? Puntos { get; set; } // Para almacenar los puntos del jugador

    }

    public class AtributosJugador
    {
        public static ResultadosAtributos ObtenerPuntosdelJugador()
        {
            var resultado = new ResultadosAtributos();

            try
            {
                // Verificar si el jugador ha iniciado sesión
                if (!LoginPanel.idJugador.HasValue) // si el idJugador es null es porque no se ha iniciado sesión
                {
                    resultado.Titulo = "Puntos bGames";
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


                    var datos = Regex.Matches(result, @"""data""\s*:\s*(\d+)"); // Expresión regular para extraer los datos buscando el valor de "data"
                    int sumaPuntos = 0;
                    foreach (Match match in datos) //Recorre los datos encontrados para luego sumar los puntos
                    {
                        if (int.TryParse(match.Groups[1].Value, out int valor))
                        {
                            sumaPuntos += valor;
                        }
                    }
                    if (datos.Count > 0)
                    {
                        
                        resultado.Titulo = "Puntos bGames";
                        resultado.Puntos = sumaPuntos;
                        resultado.Mensaje = sumaPuntos == 1
                            ? $"Tienes {sumaPuntos} punto bGames."
                            : $"Tienes {sumaPuntos} puntos bGames.";

                    }
                    else
                    {
                        resultado.Titulo = "Sin puntos bGames";
                        resultado.Mensaje = "No cuentas con puntos bGames.";
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

