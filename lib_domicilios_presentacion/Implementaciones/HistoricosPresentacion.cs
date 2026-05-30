
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Interfaces;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class HistoricosPresentacion : IHistoricosPresentacion
    {
        private Comunicaciones iComunicaciones;
        private string iUrlBase;

        public HistoricosPresentacion()
        {
            this.iComunicaciones = new Comunicaciones();
            this.iUrlBase = "https://localhost:7065/Historicos";
        }

        public async Task<List<Historicos>> ConsultarAsync()
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Consultar" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<Historicos>>(jsonValido) ?? new List<Historicos>();
        }

        public async Task<Historicos> GuardarAsync(Historicos entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Guardar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<Historicos>(jsonValido)!;
        }
    }
}
