
using lib_domicilios_presentacion.Implementaciones;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class RolesUCom
    {
        private readonly string urlBase = "https://localhost:7065/Roles";

        [TestMethod]
        public async Task Verificar_Roles_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Roles no retornó la estructura esperada.");
        }
    }
}
