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
    }

}
