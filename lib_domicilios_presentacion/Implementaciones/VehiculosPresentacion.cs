
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Interfaces;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class VehiculosPresentacion : IVehiculosPresentacion
    {
        private Comunicaciones iComunicaciones;
        private string iUrlBase;

        public VehiculosPresentacion()
        {
            this.iComunicaciones = new Comunicaciones();
            this.iUrlBase = "https://localhost:7065/Vehiculos";
        }

        public async Task<List<Vehiculos>> ConsultarAsync()
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Consultar" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<Vehiculos>>(jsonValido) ?? new List<Vehiculos>();
        }
        public async Task<Vehiculos> ConsultarPorPlacaAsync(string placa)
        {
            if (string.IsNullOrEmpty(placa))
                throw new Exception("La placa a consultar no puede estar vacía.");

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorPlaca?placa={placa}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            var vehiculo = JsonConvert.DeserializeObject<Vehiculos>(jsonValido);
            if (vehiculo == null)
                throw new Exception($"No se encontró ningún vehículo registrado con la placa: {placa}");

            return vehiculo;
        }

        public async Task<Vehiculos> GuardarAsync(Vehiculos entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Guardar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<Vehiculos>(jsonValido)!;
        }

        public async Task<Vehiculos> ModificarAsync(Vehiculos entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Modificar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<Vehiculos>(jsonValido)!;
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
