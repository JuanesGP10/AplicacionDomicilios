using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoriasController : ControllerBase
    {
        private CategoriasNegocio? iCategoriasNegocio;

        public CategoriasController()
        {
            this.iCategoriasNegocio = new CategoriasNegocio();
        }

        [HttpGet]
        public List<Categorias> Consultar()
        {
            if (this.iCategoriasNegocio == null)
                throw new Exception("No implementado");
            return this.iCategoriasNegocio.Consultar();
        }

        [HttpPost]
        public Categorias Guardar(Categorias entidad)
        {
            if (this.iCategoriasNegocio == null)
                throw new Exception("No implementado");
            return this.iCategoriasNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Categorias Modificar(Categorias entidad)
        {
            if (this.iCategoriasNegocio == null)
                throw new Exception("No implementado");
            return this.iCategoriasNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iCategoriasNegocio == null)
                throw new Exception("No implementado");
            return this.iCategoriasNegocio.Borrar(id);
        }
    }
}
