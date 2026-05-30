
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IRastreoPedidoPresentacion
    {
        Task<List<RastreoPedido>> ConsultarAsync();
        Task<List<RastreoPedido>> ConsultarPorPedidoIdAsync(int pedidoId);
        Task<RastreoPedido> ConsultarPorIdAsync(int id);
        Task<RastreoPedido> GuardarAsync(RastreoPedido entidad);
        Task<RastreoPedido> ModificarAsync(RastreoPedido entidad);
        Task<bool> BorrarAsync(int id);
        Task<bool> ActualizarUbicacionAsync(int id, decimal nLongitud, decimal nLatitud);
    }
}
