
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IPagosPresentacion
    {
        Task<List<Pagos>> ConsultarAsync();
        Task<Pagos> ConsultarPorPedidoIdAsync(int pedidoId);
        Task<Pagos> ConsultarPorIdAsync(int id);
        Task<Pagos> GuardarAsync(Pagos entidad);
        Task<Pagos> ModificarAsync(Pagos entidad);
        Task<bool> BorrarAsync(int id);
    }
}
