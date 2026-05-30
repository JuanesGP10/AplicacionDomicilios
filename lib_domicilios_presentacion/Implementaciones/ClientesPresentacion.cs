
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Interfaces;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class ClientesPresentacion : IClientesPresentacion
    {
        private Comunicaciones iComunicaciones;
        private string iUrlBase;

        public ClientesPresentacion()
        {
            this.iComunicaciones = new Comunicaciones();
            this.iUrlBase = "https://localhost:7065/Clientes";
        }

        public async Task<List<Clientes>> ConsultarAsync()
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Consultar" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<Clientes>>(jsonValido) ?? new List<Clientes>();
        }

        public async Task<Clientes> ConsultarPorIdAsync(int id)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorId?id={id}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            var cliente = JsonConvert.DeserializeObject<Clientes>(jsonValido);
            if (cliente == null)
                throw new Exception("No se encontró el cliente solicitado.");

            return cliente;
        }

        public async Task<Clientes> ConsultarPorNombreAsync(string nombre)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorNombre?nombre={nombre}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            var cliente = JsonConvert.DeserializeObject<Clientes>(jsonValido);
            if (cliente == null)
                throw new Exception($"No se encontró ningún cliente bajo el nombre: {nombre}");

            return cliente;
        }

        public async Task<Clientes> GuardarAsync(Clientes entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Guardar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<Clientes>(jsonValido)!;
        }

        public async Task<Clientes> ModificarAsync(Clientes entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Modificar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<Clientes>(jsonValido)!;
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
    }
}
