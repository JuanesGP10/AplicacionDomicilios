
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class PedidosUCom
    {
        private readonly string urlBase = "https://localhost:7065/Pedidos";

        [TestMethod]
        public async Task Verificar_Pedidos_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Pedidos no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_Pedidos()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            var datosPost = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { Id = 0, ClienteId = 3, RepartidorId = 6, EstadoPedidoId = 1, FechaCreacion = DateTime.Now, Total = 50000.00m } }
            };
            var resultadoPost = await comunicaciones.Ejecutar(datosPost);

            Assert.IsNotNull(resultadoPost);
            Assert.IsTrue(resultadoPost.ContainsKey("valor"));

            string jsonObjetoCreado = resultadoPost["valor"].ToString();
            var objetoCreado = JsonConvert.DeserializeObject<Pedidos>(jsonObjetoCreado);
            int IdGenerado = objetoCreado.Id;

            Assert.IsTrue(IdGenerado > 0);

            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new { Id = IdGenerado, ClienteId = 3, RepartidorId = 6, EstadoPedidoId = 1, FechaCreacion = DateTime.Now, Total = 55000.00m } }
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
