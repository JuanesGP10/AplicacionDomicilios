
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace MST_Unitarias.ComunicacionesUnitarias
{

    [TestClass]
    public sealed class CalificacionesUCom
    {
        private readonly string urlBase = "https://localhost:7065/Calificaciones";

        [TestMethod]
        public async Task Verificar_Calificaciones_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Calificaciones no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_Calificaciones()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            // 1. POST (Guardar)
            var datosPost = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { Id = 0, PedidoId = 1, Puntaje = 5, Comentario = "Excelente servicio en la prueba", Fecha = DateTime.Now } }
            };
            var resultadoPost = await comunicaciones.Ejecutar(datosPost);

            Assert.IsNotNull(resultadoPost, "El guardar no retornó respuesta.");
            Assert.IsTrue(resultadoPost.ContainsKey("valor"), "El guardar no retornó la clave 'valor'.");

            string jsonObjetoCreado = resultadoPost["valor"].ToString();
            var objetoCreado = JsonConvert.DeserializeObject<Calificaciones>(jsonObjetoCreado); // Mapeo a Calificaciones
            int IdGenerado = objetoCreado.Id;

            Assert.IsTrue(IdGenerado > 0, "La base de datos no generó un ID válido para la prueba.");

            // 2. PUT (Modificar)
            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new { Id = IdGenerado, PedidoId = 1, Puntaje = 4, Comentario = "Servicio modificado en la prueba", Fecha = DateTime.Now } }
            };
            await comunicaciones.Ejecutar(datosPut);

            // 3. DELETE (Borrar por QueryString)
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
