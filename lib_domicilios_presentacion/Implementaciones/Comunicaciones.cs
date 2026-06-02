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
                var url = datos["Url"].ToString() ?? "";
                datos.Remove("Url");

                string urlMinuscula = url.ToLower();
                string metodo = "GET"; 

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

                var stringData = datos.ContainsKey("Entidad") ?
                    JsonConvert.SerializeObject(datos["Entidad"]) : "{}";

                var body = new StringContent(stringData, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                var httpClient = new HttpClient(handler);
                httpClient.Timeout = new TimeSpan(0, 4, 0);

                HttpResponseMessage message;

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
