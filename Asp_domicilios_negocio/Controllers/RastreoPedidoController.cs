using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RastreoPedidoController : ControllerBase
    {
        private RastreoPedidoNegocio? iRastreoPedidoNegocio;

        public RastreoPedidoController()
        {
            this.iRastreoPedidoNegocio = new RastreoPedidoNegocio();
        }

        [HttpGet]
        public List<RastreoPedido> Consultar()
        {
            if (this.iRastreoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoPedidoNegocio.Consultar();
        }

        [HttpGet]
        public List<RastreoPedido> ConsultarPorPedidoId(int pedidoId)
        {
            if (this.iRastreoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoPedidoNegocio.ConsultarPorPedidoId(pedidoId);
        }

        [HttpGet]
        public RastreoPedido ConsultarPorId(int id)
        {
            if (this.iRastreoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoPedidoNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public RastreoPedido Guardar(RastreoPedido entidad)
        {
            if (this.iRastreoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoPedidoNegocio.Guardar(entidad);
        }

        [HttpPost]
        public bool ActualizarUbicacion(int repartidorId, decimal latitud, decimal longitud)
        {
            if (this.iRastreoPedidoNegocio == null)
                throw new Exception("No implementado");

            return this.iRastreoPedidoNegocio.ActualizarUbicacion(repartidorId, latitud, longitud);
        }

        [HttpPut]
        public RastreoPedido Modificar(RastreoPedido entidad)
        {
            if (this.iRastreoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoPedidoNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iRastreoPedidoNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoPedidoNegocio.Borrar(id);
        }
    }
}
