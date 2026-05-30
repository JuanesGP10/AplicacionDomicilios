using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ZonasController : ControllerBase
    {
        private ZonasNegocio? iZonasNegocio;

        public ZonasController()
        {
            this.iZonasNegocio = new ZonasNegocio();
        }

        [HttpGet]
        public List<Zonas> Consultar()
        {
            if (this.iZonasNegocio == null)
                throw new Exception("No implementado");
            return this.iZonasNegocio.Consultar();
        }

        [HttpPost]
        public Zonas Guardar(Zonas entidad)
        {
            if (this.iZonasNegocio == null)
                throw new Exception("No implementado");
            return this.iZonasNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Zonas Modificar(Zonas entidad)
        {
            if (this.iZonasNegocio == null)
                throw new Exception("No implementado");
            return this.iZonasNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iZonasNegocio == null)
                throw new Exception("No implementado");
            return this.iZonasNegocio.Borrar(id);
        }
    }
}
