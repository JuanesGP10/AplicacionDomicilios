
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class ClientesComunicacionesUnitaria
    {
        private readonly string urlBase = "https://localhost:7065/Clientes";

        [TestMethod]
        public async Task Verificar_Clientes_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Clientes no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_Clientes()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            var datosPost = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { Id = 0, Cedula = "123456789", Nombre = "Cliente Prueba", Email = "clientetest@domicilios.com", Contrasena = "Segura123!", FechaNacimiento = new DateTime(2000, 1, 1), Rol = 2, Direccion = "Calle 10 # 5-10", Telefono = "3005554433", FechaRegistro = DateTime.Now, MetodoPagoFav = 1, Activo = true } }
            };
            var resultadoPost = await comunicaciones.Ejecutar(datosPost);

            Assert.IsNotNull(resultadoPost);
            Assert.IsTrue(resultadoPost.ContainsKey("valor"));

            string jsonObjetoCreado = resultadoPost["valor"].ToString();
            var objetoCreado = JsonConvert.DeserializeObject<Clientes>(jsonObjetoCreado);
            int IdGenerado = objetoCreado.Id;

            Assert.IsTrue(IdGenerado > 0);

            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new { Id = IdGenerado, Cedula = "123456789", Nombre = "Cliente Modificado", Email = "clientetest@domicilios.com", Contrasena = "Segura123!", FechaNacimiento = new DateTime(2000, 1, 1), Rol = 2, Direccion = "Avenida Siempre Viva 742", Telefono = "3117778899", FechaRegistro = DateTime.Now, MetodoPagoFav = 1, Activo = true } }
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
