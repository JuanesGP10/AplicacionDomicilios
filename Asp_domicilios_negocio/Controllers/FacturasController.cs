using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FacturasController : ControllerBase
    {
        private FacturasNegocio? iFacturasNegocio;

        public FacturasController()
        {
            this.iFacturasNegocio = new FacturasNegocio();
        }

        [HttpGet]
        public List<Facturas> Consultar()
        {
            if (this.iFacturasNegocio == null)
                throw new Exception("No implementado");
            return this.iFacturasNegocio.Consultar();
        }

        [HttpGet]
        public List<Facturas> ConsultarPorPedidoId(int pedidoId)
        {
            if (this.iFacturasNegocio == null)
                throw new Exception("No implementado");
            return this.iFacturasNegocio.ConsultarPorPedidoId(pedidoId);
        }

        [HttpGet]
        public Facturas ConsultarPorId(int id)
        {
            if (this.iFacturasNegocio == null)
                throw new Exception("No implementado");
            return this.iFacturasNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public Facturas Guardar(Facturas entidad)
        {
            if (this.iFacturasNegocio == null)
                throw new Exception("No implementado");
            return this.iFacturasNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Facturas Modificar(Facturas entidad)
        {
            if (this.iFacturasNegocio == null)
                throw new Exception("No implementado");
            return this.iFacturasNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iFacturasNegocio == null)
                throw new Exception("No implementado");
            return this.iFacturasNegocio.Borrar(id);
        }
    }
}
