
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IEstadoPedidoPresentacion
    {
        Task<List<EstadoPedido>> ConsultarAsync();
        Task<EstadoPedido> GuardarAsync(EstadoPedido entidad);
        Task<EstadoPedido> ModificarAsync(EstadoPedido entidad);
        Task<bool> BorrarAsync(int id);
    }
}
