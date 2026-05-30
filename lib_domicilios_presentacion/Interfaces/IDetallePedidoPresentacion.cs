
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IDetallePedidoPresentacion
    {
        Task<List<DetallePedido>> ConsultarAsync();
        Task<List<DetallePedido>> ConsultarPorPedidoIdAsync(int pedidoId);
        Task<DetallePedido> GuardarAsync(DetallePedido entidad);
        Task<DetallePedido> ModificarAsync(DetallePedido entidad);
        Task<bool> BorrarAsync(int id);
    }
}
