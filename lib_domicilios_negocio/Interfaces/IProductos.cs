using lib_domicilios_negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IProductos
    {
        List<Productos> Consultar();
        List<Productos> ConsultarPorNombre(string Nombre);
        List<Productos> ConsultarPorCategoria(int categoriaId);
        Productos Guardar(Productos entidad);
        Productos Modificar(Productos entidad);
        bool Borrar(int id);
    }
}
