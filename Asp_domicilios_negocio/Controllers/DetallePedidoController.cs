using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DetallePedidoController : ControllerBase
    {
        private DetallePedidoNegocio? iDetallePedidoNegocio;

        public DetallePedidoController()
        {
            this.iDetallePedidoNegocio = new DetallePedidoNegocio();
        }

        [HttpGet]
        public List<DetallePedido> Consultar()
        {
            if (this.iDetallePedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iDetallePedidoNegocio.Consultar();
        }

        [HttpGet]
        public List<DetallePedido> ConsultarPorPedidoId(int pedidoId)
        {
            if (this.iDetallePedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iDetallePedidoNegocio.ConsultarPorPedidoId(pedidoId);
        }

        [HttpPost]
        public DetallePedido Guardar(DetallePedido entidad)
        {
            if (this.iDetallePedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iDetallePedidoNegocio.Guardar(entidad);
        }

        [HttpPut]
        public DetallePedido Modificar(DetallePedido entidad)
        {
            if (this.iDetallePedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iDetallePedidoNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iDetallePedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iDetallePedidoNegocio.Borrar(id);
        }
    }
}
