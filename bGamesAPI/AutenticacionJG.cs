using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace bGamesAPI
{
    public class ResultadosAutenticacion
    {
        public string Titulo { get; set; } // Para almacenar el título del resultado
        public string Mensaje { get; set; } // Para almacenar el mensaje del resultado

        public int? IdJugador { get; set; } // Para almacenar el ID del jugador

    }

    public class AutenticacionJG
    {
        public static ResultadosAutenticacion IniciarSesion(string email, string contrasena)
        {
            var resultado = new ResultadosAutenticacion();
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
                        return resultado;
                    }
                    // Verifica si la contraseña es correcta 
                    if (password.Success && password.Groups[1].Value == contrasena)
                    {
                        resultado.Titulo = "Inicio de sesión exitoso";
                        resultado.Mensaje = "Sesión iniciada correctamente.";
                        resultado.IdJugador = int.Parse(idplayer.Groups[1].Value); // Almacena el ID del jugador
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
