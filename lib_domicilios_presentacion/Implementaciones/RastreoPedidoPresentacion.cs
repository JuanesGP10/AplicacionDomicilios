
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Interfaces;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class RastreoPedidoPresentacion : IRastreoPedidoPresentacion
    {
        private Comunicaciones iComunicaciones;
        private string iUrlBase;

        public RastreoPedidoPresentacion()
        {
            this.iComunicaciones = new Comunicaciones();
            this.iUrlBase = "https://localhost:7065/RastreoPedido";
        }

        public async Task<List<RastreoPedido>> ConsultarAsync()
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Consultar" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<RastreoPedido>>(jsonValido) ?? new List<RastreoPedido>();
        }

        public async Task<List<RastreoPedido>> ConsultarPorPedidoIdAsync(int pedidoId)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorPedidoId?pedidoId={pedidoId}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<RastreoPedido>>(jsonValido) ?? new List<RastreoPedido>();
        }

        public async Task<RastreoPedido> ConsultarPorIdAsync(int id)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorId?id={id}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            var rastreo = JsonConvert.DeserializeObject<RastreoPedido>(jsonValido);
            if (rastreo == null)
                throw new Exception("No se encontró la factura.");

            return rastreo;
        }

        public async Task<RastreoPedido> GuardarAsync(RastreoPedido entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Guardar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<RastreoPedido>(jsonValido)!;
        }

        public async Task<RastreoPedido> ModificarAsync(RastreoPedido entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Modificar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<RastreoPedido>(jsonValido)!;
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