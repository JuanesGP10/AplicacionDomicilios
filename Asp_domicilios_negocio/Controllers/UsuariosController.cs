using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UsuariosController : ControllerBase
    {
        private UsuariosNegocio? iUsuariosNegocio;

        public UsuariosController()
        {
            this.iUsuariosNegocio = new UsuariosNegocio();
        }

        [HttpGet]
        public List<Usuarios> Consultar()
        {
            if (this.iUsuariosNegocio == null)
                throw new Exception("No implementado");
            return this.iUsuariosNegocio.Consultar();
        }

        [HttpGet]
        public List<Usuarios> ConsultarPorNombre(string nombre)
        {
            if (this.iUsuariosNegocio == null)
                throw new Exception("No implementado");
            return this.iUsuariosNegocio.ConsultarPorNombre(nombre);
        }

        [HttpGet]
        public Usuarios ConsultarPorId(int id)
        {
            if (this.iUsuariosNegocio == null)
                throw new Exception("No implementado");
            return this.iUsuariosNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public Usuarios Guardar(Usuarios entidad)
        {
            if (this.iUsuariosNegocio == null)
                throw new Exception("No implementado");
            return this.iUsuariosNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Usuarios Modificar(Usuarios entidad)
        {
            if (this.iUsuariosNegocio == null)
                throw new Exception("No implementado");
            return this.iUsuariosNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iUsuariosNegocio == null)
                throw new Exception("No implementado");
            return this.iUsuariosNegocio.Borrar(id);
        }
    }
}
