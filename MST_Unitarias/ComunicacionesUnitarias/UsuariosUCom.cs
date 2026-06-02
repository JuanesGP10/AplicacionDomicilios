using lib_domicilios_presentacion.Implementaciones;

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
        public async Task Verificar_Usuarios_POST()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { Id = 0, Nombre = "Diego Zapata", RolId = 2 } }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método POST (guardar) para Usuarios no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Verificar_Usuarios_PUT()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/modificar" },
                { "Entidad", new { Id = 1, Nombre = "Diego Zapata Modificado", RolId = 2 } }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método PUT (modificar) para Usuarios no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Verificar_Usuarios_DELETE()
        {
            Comunicaciones comunicaciones = new Comunicaciones();

            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/borrar" },
                { "Entidad", new { Id = 1 } }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método DELETE (borrar) para Usuarios no retornó la estructura esperada.");
        }
    }
}
