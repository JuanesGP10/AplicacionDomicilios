using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class VehiculosController : ControllerBase
    {
        private VehiculosNegocio? iVehiculosNegocio;

        public VehiculosController()
        {
            this.iVehiculosNegocio = new VehiculosNegocio();
        }

        [HttpGet]
        public List<Vehiculos> Consultar()
        {
            if (this.iVehiculosNegocio == null)
                throw new Exception("No implementado");
            return this.iVehiculosNegocio.Consultar();
        }

        [HttpGet]
        public Vehiculos ConsultarPorPlaca(string placa)
        {
            if (this.iVehiculosNegocio == null)
                throw new Exception("No implementado");
            return this.iVehiculosNegocio.ConsultarPorPlaca(placa);
        }

        [HttpPost]
        public Vehiculos Guardar(Vehiculos entidad)
        {
            if (this.iVehiculosNegocio == null)
                throw new Exception("No implementado");
            return this.iVehiculosNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Vehiculos Modificar(Vehiculos entidad)
        {
            if (this.iVehiculosNegocio == null)
                throw new Exception("No implementado");
            return this.iVehiculosNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iVehiculosNegocio == null)
                throw new Exception("No implementado");
            return this.iVehiculosNegocio.Borrar(id);
        }
    }
}
