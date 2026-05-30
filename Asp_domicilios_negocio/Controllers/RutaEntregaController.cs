using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RutaEntregaController : ControllerBase
    {
        private RutaEntregaNegocio? iRutaEntregaNegocio;

        public RutaEntregaController()
        {
            this.iRutaEntregaNegocio = new RutaEntregaNegocio();
        }

        [HttpGet]
        public List<RutaEntrega> Consultar()
        {
            if (this.iRutaEntregaNegocio == null)
                throw new Exception("No implementado");
            return this.iRutaEntregaNegocio.Consultar();
        }

        [HttpGet]
        public List<RutaEntrega> ConsultarPorPedidoId(int pedidoId)
        {
            if (this.iRutaEntregaNegocio == null)
                throw new Exception("No implementado");
            return this.iRutaEntregaNegocio.ConsultarPorPedidoId(pedidoId);
        }

        [HttpPost]
        public RutaEntrega Guardar(RutaEntrega entidad)
        {
            if (this.iRutaEntregaNegocio == null)
                throw new Exception("No implementado");
            return this.iRutaEntregaNegocio.Guardar(entidad);
        }

        [HttpPut]
        public RutaEntrega Modificar(RutaEntrega entidad)
        {
            if (this.iRutaEntregaNegocio == null)
                throw new Exception("No implementado");
            return this.iRutaEntregaNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iRutaEntregaNegocio == null)
                throw new Exception("No implementado");
            return this.iRutaEntregaNegocio.Borrar(id);
        }
    }
}