using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EstadoPagoController : ControllerBase
    {
        private EstadoPagoNegocio? iEstadoPagoNegocio;

        public EstadoPagoController()
        {
            this.iEstadoPagoNegocio = new EstadoPagoNegocio();
        }

        [HttpGet]
        public List<EstadoPago> Consultar()
        {
            if (this.iEstadoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPagoNegocio.Consultar();
        }

        [HttpPost]
        public EstadoPago Guardar(EstadoPago entidad)
        {
            if (this.iEstadoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPagoNegocio.Guardar(entidad);
        }

        [HttpPut]
        public EstadoPago Modificar(EstadoPago entidad)
        {
            if (this.iEstadoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPagoNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iEstadoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iEstadoPagoNegocio.Borrar(id);
        }
    }
}
