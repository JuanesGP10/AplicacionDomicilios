using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RastreoRepartidorController : ControllerBase
    {
        private RastreoRepartidorNegocio? iRastreoRepartidorNegocio;

        public RastreoRepartidorController()
        {
            this.iRastreoRepartidorNegocio = new RastreoRepartidorNegocio();
        }

        [HttpGet]
        public List<RastreoRepartidor> Consultar()
        {
            if (this.iRastreoRepartidorNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoRepartidorNegocio.Consultar();
        }

        [HttpGet]
        public RastreoRepartidor ConsultarPorRepartidorId(int repartidorId)
        {
            if (this.iRastreoRepartidorNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoRepartidorNegocio.ConsultarPorRepartidorId(repartidorId);
        }

        [HttpGet]
        public RastreoRepartidor ConsultarPorId(int id)
        {
            if (this.iRastreoRepartidorNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoRepartidorNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public RastreoRepartidor Guardar(RastreoRepartidor entidad)
        {
            if (this.iRastreoRepartidorNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoRepartidorNegocio.Guardar(entidad);
        }

        [HttpPut]
        public RastreoRepartidor Modificar(RastreoRepartidor entidad)
        {
            if (this.iRastreoRepartidorNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoRepartidorNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iRastreoRepartidorNegocio == null)
                throw new Exception("No implementado");
            return this.iRastreoRepartidorNegocio.Borrar(id);
        }

        [HttpPost]
        public bool ActualizarUbicacion(int repartidorId, decimal latitud, decimal longitud)
        {
            if (this.iRastreoRepartidorNegocio == null)
                throw new Exception("No implementado");

            return this.iRastreoRepartidorNegocio.ActualizarUbicacion(repartidorId, latitud, longitud);
        }
    }
}
