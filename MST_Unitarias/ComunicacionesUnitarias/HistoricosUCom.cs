
using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_presentacion.Implementaciones;

namespace MST_Unitarias.ComunicacionesUnitarias
{
    [TestClass]
    public sealed class HistoricosUCom
    {
        private readonly string urlBase = "https://localhost:7065/Historicos";

        [TestMethod]
        public async Task Verificar_Historicos_GET()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/consultar" }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método GET para Históricos no retornó la estructura esperada.");
        }

        [TestMethod]
        public async Task Verificar_Historicos_POST()
        {
            Comunicaciones comunicaciones = new Comunicaciones();
            Dictionary<string, object> datos = new Dictionary<string, object>
            {
                { "Url", $"{urlBase}/guardar" },
                { "Entidad", new { Id = 0, EntidadAfectada = "Pedidos", Descripcion = "Creacion de pedido exitosa", Fecha = DateTime.Now } }
            };

            var resultado = await comunicaciones.Ejecutar(datos);

            if (resultado != null && resultado.ContainsKey("valor"))
                return;

            throw new Exception("El método POST (guardar) para Históricos no retornó la estructura esperada.");
        }
    }
}