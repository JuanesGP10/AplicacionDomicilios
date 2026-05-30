using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PedidosController : ControllerBase
    {
        private PedidosNegocio? iPedidosNegocio;

        public PedidosController()
        {
            this.iPedidosNegocio = new PedidosNegocio();
        }

        [HttpGet]
        public List<Pedidos> Consultar()
        {
            if (this.iPedidosNegocio == null)
                throw new Exception("No implementado");
            return this.iPedidosNegocio.Consultar();
        }

        [HttpGet]
        public Pedidos ConsultarPorId(int id)
        {
            if (this.iPedidosNegocio == null)
                throw new Exception("No implementado");
            return this.iPedidosNegocio.ConsultarPorId(id);
        }

        [HttpGet]
        public List<Pedidos> ConsultarPorCliente(int clienteId)
        {
            if (this.iPedidosNegocio == null)
                throw new Exception("No implementado");
            return this.iPedidosNegocio.ConsultarPorCliente(clienteId);
        }

        [HttpPost]
        public Pedidos Guardar(Pedidos entidad)
        {
            if (this.iPedidosNegocio == null)
                throw new Exception("No implementado");
            return this.iPedidosNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Pedidos Modificar(Pedidos entidad)
        {
            if (this.iPedidosNegocio == null)
                throw new Exception("No implementado");
            return this.iPedidosNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iPedidosNegocio == null)
                throw new Exception("No implementado");
            return this.iPedidosNegocio.Borrar(id);
        }
    }
}
