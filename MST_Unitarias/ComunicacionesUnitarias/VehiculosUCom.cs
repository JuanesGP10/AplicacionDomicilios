
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class VehiculosUCom
    {
        private readonly string urlBase = "https://localhost:7065/Vehiculos";

        [TestMethod]
        public async Task Verificar_Vehiculos_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Vehículos no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_Vehiculos()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            var datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new {Id = 0, Placa = "RTY456", Tipo="Moto",Modelo = "2023",Activo=true } }
            };
            var resultadoPost = await comunicaciones.Ejecutar(datos);

            Assert.IsNotNull(resultadoPost);
            Assert.IsTrue(resultadoPost.ContainsKey("valor"));

            string jsonObjetoCreado = resultadoPost["valor"].ToString();
            var objetoCreado = JsonConvert.DeserializeObject<Vehiculos>(jsonObjetoCreado);
            int IdGenerado = objetoCreado.Id;

            Assert.IsTrue(IdGenerado > 0);

            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new { Id = IdGenerado, Placa = "XYZ-789",Tipo = "Moto", Modelo = "2026", Activo = true } }
            };
            await comunicaciones.Ejecutar(datosPut);

            var datosDelete = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/borrar?id={IdGenerado}" }
            };
            var resultadoDelete = await comunicaciones.Ejecutar(datosDelete);

            Assert.IsNotNull(resultadoDelete);
            Assert.IsTrue(resultadoDelete.ContainsKey("valor"));
        }
    }
}
