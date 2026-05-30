
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IEstadoPagoPresentacion
    {
        Task<List<EstadoPago>> ConsultarAsync();
        Task<EstadoPago> GuardarAsync(EstadoPago entidad);
        Task<EstadoPago> ModificarAsync(EstadoPago entidad);
        Task<bool> BorrarAsync(int id);
    }
}
