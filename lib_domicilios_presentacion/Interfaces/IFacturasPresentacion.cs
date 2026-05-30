
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IFacturasPresentacion
    {
    Task<List<Facturas>> ConsultarAsync();
    Task<List<Facturas>> ConsultarPorPedidoIdAsync(int pedidoId);
    Task<Facturas> ConsultarPorIdAsync(int id);
    Task<Facturas> GuardarAsync(Facturas entidad);
    Task<Facturas> ModificarAsync(Facturas entidad);
    Task<bool> BorrarAsync(int id);
    }
}
