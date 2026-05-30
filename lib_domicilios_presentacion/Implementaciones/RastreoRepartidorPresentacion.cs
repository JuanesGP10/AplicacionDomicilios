
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Interfaces;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class RastreoRepartidorPresentacion : IRastreoRepartidorPresentacion
    {
        private Comunicaciones iComunicaciones;
        private string iUrlBase;

        public RastreoRepartidorPresentacion()
        {
            this.iComunicaciones = new Comunicaciones();
            this.iUrlBase = "https://localhost:7065/RastreoRepartidor";
        }

        public async Task<List<RastreoRepartidor>> ConsultarAsync()
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Consultar" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<RastreoRepartidor>>(jsonValido) ?? new List<RastreoRepartidor>();
        }

        public async Task<RastreoRepartidor> ConsultarPorRepartidorIdAsync(int repartidorId)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorRepartidorId?repartidorId={repartidorId}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            var rastreo = JsonConvert.DeserializeObject<RastreoRepartidor>(jsonValido);
            if (rastreo == null)
                throw new Exception($"No se encontró información de rastreo para el repartidor ID: {repartidorId}");

            return rastreo;
        }

        public async Task<RastreoRepartidor> ConsultarPorIdAsync(int id)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorId?id={id}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            var rastreo = JsonConvert.DeserializeObject<RastreoRepartidor>(jsonValido);
            if (rastreo == null)
                throw new Exception("No se encontró la factura.");

            return rastreo;
        }

        public async Task<RastreoRepartidor> GuardarAsync(RastreoRepartidor entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Guardar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<RastreoRepartidor>(jsonValido)!;
        }

        public async Task<RastreoRepartidor> ModificarAsync(RastreoRepartidor entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Modificar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<RastreoRepartidor>(jsonValido)!;
        }

        public async Task<bool> BorrarAsync(int id)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Borrar?id={id}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<bool>(jsonValido);
        }

        public async Task<bool> ActualizarUbicacionAsync(int id, decimal nLongitud, decimal nLatitud)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ActualizarUbicacion?id={id}&nLongitud={nLongitud}&nLatitud={nLatitud}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<bool>(jsonValido);
        }
    }
}
