using Newtonsoft.Json;
using System.Text;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class Comunicaciones
    {
        public async Task<Dictionary<string, object>> Ejecutar(Dictionary<string, object> datos)
        {
            try
            {
                // 1. Extraer y quitar la URL para limpiar el diccionario
                var url = datos["Url"].ToString() ?? "";
                datos.Remove("Url");

                // 2. DETECCIÓN AUTOMÁTICA DEL MÉTODO BASADO EN LA URL
                // Convertimos a minúsculas la URL para evitar problemas de mayúsculas/minúsculas
                string urlMinuscula = url.ToLower();
                string metodo = "GET"; // Por defecto asumimos consulta

                if (urlMinuscula.Contains("guardar"))
                {
                    metodo = "POST";
                }
                else if (urlMinuscula.Contains("modificar"))
                {
                    metodo = "PUT";
                }
                else if (urlMinuscula.Contains("borrar"))
                {
                    metodo = "DELETE";
                }
                else
                {
                   metodo = "GET";
                }

                // 3. Serializar la entidad si existe
                var stringData = datos.ContainsKey("Entidad") ?
                    JsonConvert.SerializeObject(datos["Entidad"]) : "{}";

                var body = new StringContent(stringData, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                // 4. Configurar el cliente HTTP
                var httpClient = new HttpClient(handler);
                httpClient.Timeout = new TimeSpan(0, 4, 0);

                HttpResponseMessage message;

                // 5. DISPARAR LA PETICIÓN SEGÚN EL MÉTODO DETECTADO
                switch (metodo)
                {
                    case "POST":
                        message = await httpClient.PostAsync(url, body);
                        break;
                    case "PUT":
                        message = await httpClient.PutAsync(url, body);
                        break;
                    case "DELETE":
                        var request = new HttpRequestMessage(HttpMethod.Delete, url)
                        {
                            Content = body
                        };
                        message = await httpClient.SendAsync(request);
                        break;
                    case "GET":
                    default:
                        message = await httpClient.GetAsync(url);
                        break;
                }

                // 6. Validar respuesta del servidor
                if (!message.IsSuccessStatusCode)
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    throw new Exception($"Error Comunicacion: {message.StatusCode} - {errorContent}");
                }

                var resp = await message.Content.ReadAsStringAsync();
                httpClient.Dispose(); httpClient = null;

                if (string.IsNullOrEmpty(resp))
                    return new Dictionary<string, object>();

                resp = Replace(resp);
                return new Dictionary<string, object>() { { "valor", resp } };
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo en la capa de comunicaciones: " + ex.Message);
            }
        }

        public object Ejecutar<T>(Dictionary<string, object> datos)
        {
            throw new NotImplementedException();
        }

        private string Replace(string resp)
        {
            return resp.Replace("\\\\r\\\\n", "")
                .Replace("\\r\\n", "")
                .Replace("\\", "")
                .Replace("\\\"", "\"")
                .Replace("\"", "'")
                .Replace("'[", "[")
                .Replace("]'", "]")
                .Replace("'{'", "{'")
                .Replace("\\\\", "\\")
                .Replace("'}'", "'}")
                .Replace("}'", "}")
                .Replace("\\n", "")
                .Replace("\\r", "")
                .Replace("    ", "")
                .Replace("'{", "{")
                .Replace("\"", "")
                .Replace("  ", "")
                .Replace("null", "''");
        }
    }
}
