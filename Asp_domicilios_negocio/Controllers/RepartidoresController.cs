using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RepartidoresController : ControllerBase
    {
        private RepartidoresNegocio? iRepartidoresNegocio;

        public RepartidoresController()
        {
            this.iRepartidoresNegocio = new RepartidoresNegocio();
        }

        [HttpGet]
        public List<Repartidores> Consultar()
        {
            if (this.iRepartidoresNegocio == null)
                throw new Exception("No implementado");
            return this.iRepartidoresNegocio.Consultar();
        }

        [HttpGet]
        public List<Repartidores> ConsultarPorNombre(string nombre)
        {
            if (this.iRepartidoresNegocio == null)
                throw new Exception("No implementado");
            return this.iRepartidoresNegocio.ConsultarPorNombre(nombre);
        }

        [HttpPost]
        public Repartidores Guardar(Repartidores entidad)
        {
            if (this.iRepartidoresNegocio == null)
                throw new Exception("No implementado");
            return this.iRepartidoresNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Repartidores Modificar(Repartidores entidad)
        {
            if (this.iRepartidoresNegocio == null)
                throw new Exception("No implementado");
            return this.iRepartidoresNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iRepartidoresNegocio == null)
                throw new Exception("No implementado");
            return this.iRepartidoresNegocio.Borrar(id);
        }
    }
}
