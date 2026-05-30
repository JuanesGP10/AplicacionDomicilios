
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IPedidosPresentacion
    {
        Task<List<Pedidos>> ConsultarAsync();
        Task<Pedidos> ConsultarPorIdAsync(int id);
        Task<List<Pedidos>> ConsultarPorClienteIdAsync(int clienteId);
        Task<Pedidos> GuardarAsync(Pedidos entidad);
        Task<Pedidos> ModificarAsync(Pedidos entidad);
        Task<bool> BorrarAsync(int id);
        Task<bool> ActualizarEstadoAsync(int pedidoId, int estadoId);
    }
}
