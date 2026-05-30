
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IUsuariosPresentacion
    {
        Task<List<Usuarios>> ConsultarAsync();
        Task<Usuarios> ConsultarPorIdAsync(int id);
        Task<Usuarios> ConsultarPorNombreAsync(string nombre);
        Task<Usuarios> GuardarAsync(Usuarios entidad);
        Task<Usuarios> ModificarAsync(Usuarios entidad);
        Task<bool> BorrarAsync(int id);
    }
}