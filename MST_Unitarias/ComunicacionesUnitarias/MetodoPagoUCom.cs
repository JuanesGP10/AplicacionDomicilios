
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class MetodoPagoUCom
    {
        private readonly string urlBase = "https://localhost:7065/MetodoPago";

        [TestMethod]
        public async Task Verificar_MetodoPago_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para MetodoPago no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_MetodoPago()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            var datosPost = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { Id = 0, Nombre = "Efectivo Prueba", Descripcion = "Pago contra entrega", Comision = 0.00m, Activo = true } }
            };
            var resultadoPost = await comunicaciones.Ejecutar(datosPost);

            Assert.IsNotNull(resultadoPost);
            Assert.IsTrue(resultadoPost.ContainsKey("valor"));

            string jsonObjetoCreado = resultadoPost["valor"].ToString();
            var objetoCreado = JsonConvert.DeserializeObject<MetodoPago>(jsonObjetoCreado);
            int IdGenerado = objetoCreado.Id;

            Assert.IsTrue(IdGenerado > 0);

            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new { Id = IdGenerado, Nombre = "Efectivo Modificado", Descripcion = "Pago contra entrega en efectivo", Comision = 500.00m, Activo = true } }
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