using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class UsuariosUCom
    {
        private readonly string urlBase = "https://localhost:7065/Usuarios";

        [TestMethod]
        public async Task Verificar_Usuarios_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Usuarios no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Ciclo_POST_PUT_DELETE_Usuarios()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            var datosPost = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new {
                    Id = 0,
                    Cedula = "555666777",
                    Nombre = "Usuario Prueba",
                    Email = "usuario.test@domicilios.com",
                    Contrasena = "Usuario123*",
                    FechaNacimiento = new DateTime(1998, 10, 10),
                    Rol = 1 
                } }
            };

            var resultadoPost = await comunicaciones.Ejecutar(datosPost);
            Assert.IsNotNull(resultadoPost);

            string jsonObjetoCreado = resultadoPost["valor"].ToString();
            var usuarioCreado = JsonConvert.DeserializeObject<Usuarios>(jsonObjetoCreado);
            int IdGenerado = usuarioCreado.Id;
            Assert.IsTrue(IdGenerado > 0);

            var datosPut = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new {
                    Id = IdGenerado,
                    Cedula = "555666777",
                    Nombre = "Usuario Editado",
                    Email = "usuario.test@domicilios.com",
                    Contrasena = "Usuario123*",
                    FechaNacimiento = new DateTime(1998, 10, 10),
                    Rol = 1
                } }
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
