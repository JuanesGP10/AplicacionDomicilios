
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface ICategorias
    {
        List<Categorias> Consultar();
        Categorias Guardar(Categorias entidad);
        Categorias Modificar(Categorias entidad);
        bool Borrar(int id);
    }
}
