
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IRastreoPedido
    {
        List<RastreoPedido> Consultar();
        List<RastreoPedido> ConsultarPorPedidoId(int pedidoId);
        RastreoPedido ConsultarPorId(int id);
        RastreoPedido Guardar(RastreoPedido entidad);
        RastreoPedido Modificar(RastreoPedido entidad);
        bool Borrar(int id);
        bool ActualizarUbicacion(int id, decimal nLongitud, decimal nLatitud);
    }
}
