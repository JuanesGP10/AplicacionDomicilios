
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IRolesPresentacion
    {
        Task<List<Roles>> ConsultarAsync();
        Task<Roles> ConsultarPorNombreAsync(string nombre);
    }
}
