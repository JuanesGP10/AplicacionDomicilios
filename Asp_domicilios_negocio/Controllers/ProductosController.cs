using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductosController : ControllerBase
    {
        private ProductosNegocio? iProductosNegocio;

        public ProductosController()
        {
            this.iProductosNegocio = new ProductosNegocio();
        }

        [HttpGet]
        public List<Productos> Consultar()
        {
            if (this.iProductosNegocio == null)
                throw new Exception("No implementado");
            return this.iProductosNegocio.Consultar();
        }

        [HttpGet]
        public List<Productos> ConsultarPorNombre(string nombre)
        {
            if (this.iProductosNegocio == null)
                throw new Exception("No implementado");
            return this.iProductosNegocio.ConsultarPorNombre(nombre);
        }

        [HttpGet]
        public List<Productos> ConsultarPorCategoria(int categoriaId)
        {
            if (this.iProductosNegocio == null)
                throw new Exception("No implementado");
            return this.iProductosNegocio.ConsultarPorCategoria(categoriaId);
        }


        [HttpPost]
        public Productos Guardar(Productos entidad)
        {
            if (this.iProductosNegocio == null)
                throw new Exception("No implementado");
            return this.iProductosNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Productos Modificar(Productos entidad)
        {
            if (this.iProductosNegocio == null)
                throw new Exception("No implementado");
            return this.iProductosNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iProductosNegocio == null)
                throw new Exception("No implementado");
            return this.iProductosNegocio.Borrar(id);
        }
    }
}
