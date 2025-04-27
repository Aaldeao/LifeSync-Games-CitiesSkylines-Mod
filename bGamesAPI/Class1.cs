using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

using ModCitiesSkylines;

namespace bGamesAPI
{
    public class ResultadosAPI
    {
        public string Titulo { get; set; } // Para almacenar el título del resultado
        public string Mensaje { get; set; } // Para almacenar el mensaje del resultado

    }

    public static class APIbGames
    {
        //Metodo probar la conexión con la API de bGames
        public static ResultadosAPI ConexionAPI()
        {
            var resultado = new ResultadosAPI();
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
                    resultado.Titulo = "Conexión Exitosa";
                    resultado.Mensaje = "Respuesta del servidor: " + result;
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción si la conexión falla
                resultado.Titulo = "Error de conexión";
                resultado.Mensaje = ex.Message;
            }
            return resultado; // Devolver el resultado de la conexión

        }

        //Metodo para obtener los puntos del jugador
        public static ResultadosAPI ObtenerPuntosdelJugador()
        {
            var resultado = new ResultadosAPI();

            try
            {
                // Verificar si el jugador ha iniciado sesión
                if (!LoginPanel.idJugador.HasValue) // si el idJugador es null es porque no se ha iniciado sesión
                {
                    resultado.Titulo = "Puntos de Usuario";
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
                        resultado.Titulo = "Puntos de Usuario";
                        resultado.Mensaje = "Puntos: " + sumaPuntos;
                    }
                    else
                    {
                        resultado.Titulo = "Sin datos";
                        resultado.Mensaje = "No se encontraron datos del jugador.";
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

        public static ResultadosAPI IniciarSesion(string email, string contrasena)
        {
            var resultado = new ResultadosAPI();
            try
            {
                string url = $"http://localhost:3010/player_by_email/{email}"; // URL de la API de bGames
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {

                    string result = reader.ReadToEnd();

                    var password = Regex.Match(result, @"""password""\s*:\s*""(.*?)""");
                    var idplayer = Regex.Match(result, @"""id_players""\s*:\s*(\d+)");

                    // Verificar si el correo electrónico existe
                    if (!idplayer.Success)
                    {
                        resultado.Titulo = "Inicio de sesión";
                        resultado.Mensaje = "Correo no encontrado.";
                    }
                    // Verifica si la contraseña es correcta 
                    else if (password.Success && password.Groups[1].Value == contrasena)
                    {
                        resultado.Titulo = "Inicio de sesión";
                        resultado.Mensaje = idplayer.Groups[1].Value;
                    }
                    // Si la contraseña no es correcta
                    else
                    {
                        resultado.Titulo = "Inicio de sesión";
                        resultado.Mensaje = "Contraseña incorrecta.";

                    }
                }
            }
            catch (Exception ex)
            {
                resultado.Titulo = "Error de conexión";
                resultado.Mensaje = ex.Message;
            }

            return resultado;

        }
    }
}
