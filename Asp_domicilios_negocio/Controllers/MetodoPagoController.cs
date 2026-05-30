using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MetodoPagoController : ControllerBase
    {
        private MetodoPagoNegocio? iMetodoPagoNegocio;

        public MetodoPagoController()
        {
            this.iMetodoPagoNegocio = new MetodoPagoNegocio();
        }

        [HttpGet]
        public List<MetodoPago> Consultar()
        {
            if (this.iMetodoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iMetodoPagoNegocio.Consultar();
        }

        [HttpGet]
        public MetodoPago ConsultarPorId(int id)
        {
            if (this.iMetodoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iMetodoPagoNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public MetodoPago Guardar(MetodoPago entidad)
        {
            if (this.iMetodoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iMetodoPagoNegocio.Guardar(entidad);
        }

        [HttpPut]
        public MetodoPago Modificar(MetodoPago entidad)
        {
            if (this.iMetodoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iMetodoPagoNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iMetodoPagoNegocio == null)
                throw new Exception("No implementado");
            return this.iMetodoPagoNegocio.Borrar(id);
        }
    }
}
