using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HistoricosController : ControllerBase
    {
        private HistoricosNegocio? iHistoricosNegocio;

        public HistoricosController()
        {
            this.iHistoricosNegocio = new HistoricosNegocio();
        }

        [HttpGet]
        public List<Historicos> Consultar()
        {
            if (this.iHistoricosNegocio == null)
                throw new Exception("No implementado");
            return this.iHistoricosNegocio.Consultar();
        }

        [HttpPost]
        public Historicos Guardar(Historicos entidad)
        {
            if (this.iHistoricosNegocio == null)
                throw new Exception("No implementado");
            return this.iHistoricosNegocio.Guardar(entidad);
        }
    }
}
