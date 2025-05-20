using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace bGamesAPI
{
    public class ResultadoCanje
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Exito { get; set; }
    }

    public class CanjearPts
    {
        public static ResultadoCanje EnviarCanje(int idJugador, int idAtributo, int nuevoValor)
        {
            var resultado = new ResultadoCanje();

            try
            {
                string url = "http://localhost:3002/player_attributes_single"; // URL de la API de bGames

                // Crea una solicitud HTTP PUT para realizar el canjeado de puntos en formato JSON
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "PUT";
                request.ContentType = "application/json";

                // Construye el JSON con los datos
                string json = $"{{\"id_player\": \"{idJugador}\", \"id_attributes\": {idAtributo}, \"new_data\": {nuevoValor} }}";

                // Convierte el JSON a un arreglo de bytes
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentLength = byteArray.Length;

                // Escribe el JSON en el cuerpo de la solicitud
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Envía la solicitud y obtiene la respuesta
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseFromServer = reader.ReadToEnd();
                    resultado.Titulo = "LifeSync Games";
                    resultado.Mensaje = "Canje realizado correctamente.";
                    resultado.Exito = true;
                }
            }
            // Manejo de excepciones
            catch (Exception ex)
            {
                resultado.Titulo = "Error de conexión";
                resultado.Mensaje = ex.Message;
                resultado.Exito = false;
            }

            return resultado;
        }
    }
}
