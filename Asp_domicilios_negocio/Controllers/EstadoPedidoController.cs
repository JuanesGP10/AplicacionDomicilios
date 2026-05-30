using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EstadoPedidoController : ControllerBase
    {
        private EstadoPedidoNegocio? iEstadoPedidoNegocio;

        public EstadoPedidoController()
        {
            this.iEstadoPedidoNegocio = new EstadoPedidoNegocio();
        }

        [HttpGet]
        public List<EstadoPedido> Consultar()
        {
            if (this.iEstadoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPedidoNegocio.Consultar();
        }

        [HttpPost]
        public EstadoPedido Guardar(EstadoPedido entidad)
        {
            if (this.iEstadoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPedidoNegocio.Guardar(entidad);
        }

        [HttpPut]
        public EstadoPedido Modificar(EstadoPedido entidad)
        {
            if (this.iEstadoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPedidoNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iEstadoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPedidoNegocio.Borrar(id);
        }
    }
}
