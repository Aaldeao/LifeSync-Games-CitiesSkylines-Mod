using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ModCitiesSkylines
{
    public class bGamesAPI // Clase para la API de bGames
    {
        public string Titulo { get; private set; }
        public string Mensaje { get; private set; }

        //Metodo probar la conexión con la API de bGames
        public void Llamar_bGames()
        {
            try
            {
                string url = "http://localhost:3001"; // URL de la API de bGames
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // Leer la respuesta del servidor si la conexión es exitosa
                    string result = reader.ReadToEnd();
                    Titulo = "Conexión Exitosa";
                    Mensaje = "Respuesta del servidor: " + result;
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción si la conexión falla
                Titulo = "Error de conexión";
                Mensaje = ex.Message;
            }
        }
    }

    public class datosJugador // Clase para obtener los datos del jugador
    {
        // Variables para almacenar los datos de la API    
        public string Mensaje { get; set; }
        public string Titulo { get; set; }

        public void ObtenerDatosdelJugador()
        {
            try
            {
                string url = "http://localhost:3001/player_all_attributes/0"; // URL de la API de bGames
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
                        Titulo = "Puntos de Usuario";
                        Mensaje = "Puntos: " + sumaPuntos;
                    }
                    else
                    {
                        Titulo = "Sin datos";
                        Mensaje = "No se encontraron datos del jugador.";
                    }

                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción si la conexión falla
                Titulo = "Error de conexión";
                Mensaje = "Error de conexión: " + ex.Message;

            }
        }

    }
}
