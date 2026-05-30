
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Interfaces;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class RolesPresentacion : IRolesPresentacion
    {
        private Comunicaciones iComunicaciones;
        private string iUrlBase;

        public RolesPresentacion()
        {
            this.iComunicaciones = new Comunicaciones();
            this.iUrlBase = "https://localhost:7065/Roles";
        }

        public async Task<List<Roles>> ConsultarAsync()
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Consultar" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);

            string jsonLimpio = resultado["valor"].ToString()!;

            string jsonValido = jsonLimpio.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<Roles>>(jsonValido) ?? new List<Roles>();
        }

        public async Task<Roles> ConsultarPorNombreAsync(string nombre)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorNombre?nombre={nombre}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonLimpio = resultado["valor"].ToString()!;
            string jsonValido = jsonLimpio.Replace("'", "\"");

            var entidad = JsonConvert.DeserializeObject<Roles>(jsonValido);
            if (entidad == null)
                throw new Exception($"No se pudo mapear el rol '{nombre}'.");

            return entidad;
        }
    }
}
