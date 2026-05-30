
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IMetodoPagoPresentacion
    {
        Task<List<MetodoPago>> ConsultarAsync();
        Task<MetodoPago> ConsultarPorIdAsync(int id);
        Task<MetodoPago> GuardarAsync(MetodoPago entidad);
        Task<MetodoPago> ModificarAsync(MetodoPago entidad);
        Task<bool> BorrarAsync(int id);
    }
}
