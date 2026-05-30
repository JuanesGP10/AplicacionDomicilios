
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Newtonsoft.Json;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IProductosPresentacion
    {
        Task<List<Productos>> ConsultarAsync();
        Task<List<Productos>> ConsultarPorNombreAsync(string nombre);
        Task<List<Productos>> ConsultarPorCategoriaIdAsync(int categoriaId);
        Task<Productos> GuardarAsync(Productos entidad);
        Task<Productos> ModificarAsync(Productos entidad);
        Task<bool> BorrarAsync(int id);
    }
}