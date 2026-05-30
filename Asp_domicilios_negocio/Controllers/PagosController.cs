using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PagosController : ControllerBase
    {
        private PagosNegocio? iPagosNegocio;

        public PagosController()
        {
            this.iPagosNegocio = new PagosNegocio();
        }

        [HttpGet]
        public List<Pagos> Consultar()
        {
            if (this.iPagosNegocio == null)
                throw new Exception("No implementado");
            return this.iPagosNegocio.Consultar();
        }

        [HttpGet]
        public List<Pagos> ConsultarPorPedidoId(int pedidoId)
        {
            if (this.iPagosNegocio == null)
                throw new Exception("No implementado");
            return this.iPagosNegocio.ConsultarPorPedidoId(pedidoId);
        }

        [HttpGet]
        public Pagos ConsultarPorId(int id)
        {
            if (this.iPagosNegocio == null)
                throw new Exception("No implementado");
            return this.iPagosNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public Pagos Guardar(Pagos entidad)
        {
            if (this.iPagosNegocio == null)
                throw new Exception("No implementado");
            return this.iPagosNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Pagos Modificar(Pagos entidad)
        {
            if (this.iPagosNegocio == null)
                throw new Exception("No implementado");
            return this.iPagosNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iPagosNegocio == null)
                throw new Exception("No implementado");
            return this.iPagosNegocio.Borrar(id);
        }
    }
}
