using System;
using System.IO;
using System.Net;

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
}
