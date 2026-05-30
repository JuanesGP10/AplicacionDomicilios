
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Interfaces;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Implementaciones
{
    public class ProductosPresentacion : IProductosPresentacion
    {
        private Comunicaciones iComunicaciones;
        private string iUrlBase;

        public ProductosPresentacion()
        {
            this.iComunicaciones = new Comunicaciones();
            this.iUrlBase = "https://localhost:7065/Productos";
        }

        public async Task<List<Productos>> ConsultarAsync()
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Consultar" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<Productos>>(jsonValido) ?? new List<Productos>();
        }

        public async Task<List<Productos>> ConsultarPorNombreAsync(string nombre)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorNombre?nombre={nombre}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<Productos>>(jsonValido) ?? new List<Productos>();
        }

        public async Task<List<Productos>> ConsultarPorCategoriaIdAsync(int categoriaId)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/ConsultarPorCategoriaId?categoriaId={categoriaId}" }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<List<Productos>>(jsonValido) ?? new List<Productos>();
        }

        public async Task<Productos> GuardarAsync(Productos entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Guardar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<Productos>(jsonValido)!;
        }

        public async Task<Productos> ModificarAsync(Productos entidad)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Url", $"{this.iUrlBase}/Modificar" },
                { "Entidad", entidad }
            };

            Dictionary<string, object> resultado = await this.iComunicaciones.Ejecutar(parametros);
            string jsonValido = resultado["valor"].ToString()!.Replace("'", "\"");

            return JsonConvert.DeserializeObject<Productos>(jsonValido)!;
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
