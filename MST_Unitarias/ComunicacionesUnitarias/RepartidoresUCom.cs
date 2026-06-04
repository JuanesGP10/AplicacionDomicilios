
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class RepartidoresUCom
    {
        private readonly string urlBase = "https://localhost:7065/Repartidores";

        [TestMethod]
        public async Task Verificar_Repartidores_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Repartidores no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_Repartidores()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            var datosPost = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { 
                    Id = 0,
                    Cedula = "1020304050",
                    Nombre = "Juan Repartidor",
                    Email = "juan.r@test.com",
                    Contrasena = "Pass1234",
                    FechaNacimiento = new DateTime(1995, 5, 20),
                    Rol = 3, 
            
                    VehiculoId = 6,
                    ZonaId = 1,   
                    Disponible = true,
                    CalificacionPromedio = 0,
                    Activo = true
                } }
            };

            var resultadoPost = await comunicaciones.Ejecutar(datosPost);
            Assert.IsNotNull(resultadoPost);

            // Deserializamos el resultado para obtener el ID generado
            string jsonObjetoCreado = resultadoPost["valor"].ToString();
            var objetoCreado = JsonConvert.DeserializeObject<Repartidores>(jsonObjetoCreado);
            int IdGenerado = objetoCreado.Id;
            Assert.IsTrue(IdGenerado > 0);

            // 2. PUT: Modificar el repartidor creado
            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new {
                    Id = IdGenerado,
                    Cedula = "1020304050",
                    Nombre = "Juan Repartidor Editado",
                    Email = "juan.r@test.com",
                    Contrasena = "Pass1234",
                    FechaNacimiento = new DateTime(1995, 5, 20),
                    Rol = 3,
                    VehiculoId = 6,
                    ZonaId = 1,
                    Disponible = false,
                    CalificacionPromedio = 4.5m,
                    Activo = true
                } }
            };
            await comunicaciones.Ejecutar(datosPut);

            // 3. DELETE: Borrar el repartidor
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
