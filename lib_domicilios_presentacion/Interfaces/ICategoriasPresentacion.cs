
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface ICategoriasPresentacion
    {
        Task<List<Categorias>> ConsultarAsync();
        Task<Categorias> GuardarAsync(Categorias entidad);
        Task<Categorias> ModificarAsync(Categorias entidad);
        Task<bool> BorrarAsync(int id);
    }
}
