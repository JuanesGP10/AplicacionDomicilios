using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RolesController : ControllerBase
    {
        private RolesNegocio? iRolesNegocio;

        public RolesController()
        {
            this.iRolesNegocio = new RolesNegocio();
        }

        [HttpGet]
        public List<Roles> Consultar()
        {
            if (this.iRolesNegocio == null)
                throw new Exception("No implementado");
            return this.iRolesNegocio.Consultar();
        }

        [HttpGet]
        public Roles ConsultarPorNombre(string nombre)
        {
            if (this.iRolesNegocio == null)
                throw new Exception("No implementado");
            return this.iRolesNegocio.ConsultarPorNombre(nombre);
        }
    }
}
