using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CalificacionesController : ControllerBase
    {
        private CalificacionesNegocio? iCalificacionesNegocio;

        public CalificacionesController()
        {
            this.iCalificacionesNegocio = new CalificacionesNegocio();
        }

        [HttpGet]
        public List<Calificaciones> Consultar()
        {
            if (this.iCalificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iCalificacionesNegocio.Consultar();
        }

        [HttpGet]
        public List<Calificaciones> ConsultarPorPedidoId(int pedidoId)
        {
            if (this.iCalificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iCalificacionesNegocio.ConsultarPorPedidoId(pedidoId);
        }

        [HttpPost]
        public Calificaciones Guardar(Calificaciones entidad)
        {
            if (this.iCalificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iCalificacionesNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Calificaciones Modificar(Calificaciones entidad)
        {
            if (this.iCalificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iCalificacionesNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iCalificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iCalificacionesNegocio.Borrar(id);
        }
    }
}
