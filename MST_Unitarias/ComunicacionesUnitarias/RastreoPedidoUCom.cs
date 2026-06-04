
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;
using System.Transactions;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class RastreoPedidoUCom
    {
        private readonly string urlBase = "https://localhost:7065/RastreoPedido";

        [TestMethod]
        public async Task Verificar_RastreoPedido_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para RastreoPedido no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_RastreoPedido()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            var datosPost = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { Id = 0, PedidoId = 1, Latitud = 6.2442m, Longitud = -75.5812m, FechaActualizacion = DateTime.Now } }
             };
            var resultadoPost = await comunicaciones.Ejecutar(datosPost);

            Assert.IsNotNull(resultadoPost, "El guardar no retornó respuesta.");
            Assert.IsTrue(resultadoPost.ContainsKey("valor"), "El guardar no retornó la clave 'valor'.");
            int IdGenerado = 0;
            string jsonObjetoCreado = resultadoPost["valor"].ToString();

            var objetoCreado = JsonConvert.DeserializeObject<RastreoPedido>(jsonObjetoCreado);
            
            IdGenerado = objetoCreado.Id;

            Assert.IsTrue(IdGenerado > 0, "La base de datos no generó un ID válido para la prueba.");

            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new { Id = IdGenerado, PedidoId = 1, Latitud = 6.2518m, Longitud = -75.5636m, FechaActualizacion = DateTime.Now } }
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
