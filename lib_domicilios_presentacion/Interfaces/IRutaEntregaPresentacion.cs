
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IRutaEntregaPresentacion
    {
        Task<List<RutaEntrega>> ConsultarAsync();
        Task<List<RutaEntrega>> ConsultarPorPedidoIdAsync(int pedidoId);
        Task<RutaEntrega> GuardarAsync(RutaEntrega entidad);
        Task<RutaEntrega> ModificarAsync(RutaEntrega entidad);
        Task<bool> BorrarAsync(int id);
    }
}
