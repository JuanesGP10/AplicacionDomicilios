using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClientesController : ControllerBase
    {
        private ClientesNegocio? iClientesNegocio;

        public ClientesController()
        {
            this.iClientesNegocio = new ClientesNegocio();
        }

        [HttpGet]
        public List<Clientes> Consultar()
        {
            if (this.iClientesNegocio == null)
                throw new Exception("No implementado");
            return this.iClientesNegocio.Consultar();
        }

        [HttpGet]
        public List<Clientes> ConsultarPorNombre(string nombre)
        {
            if (this.iClientesNegocio == null)
                throw new Exception("No implementado");
            return this.iClientesNegocio.ConsultarPorNombre(nombre);
        }

        [HttpGet]
        public Clientes ConsultarPorId(int id)
        {
            if (this.iClientesNegocio == null)
                throw new Exception("No implementado");
            return this.iClientesNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public Clientes Guardar(Clientes entidad)
        {
            if (this.iClientesNegocio == null)
                throw new Exception("No implementado");
            return this.iClientesNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Clientes Modificar(Clientes entidad)
        {
            if (this.iClientesNegocio == null)
                throw new Exception("No implementado");
            return this.iClientesNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iClientesNegocio == null)
                throw new Exception("No implementado");
            return this.iClientesNegocio.Borrar(id);
        }
    }
}
