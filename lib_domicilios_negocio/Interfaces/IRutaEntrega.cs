
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IRutaEntrega
    {
        List<RutaEntrega> Consultar();
        List<RutaEntrega> ConsultarPorPedidoId(int pedidoId);
        RutaEntrega Guardar(RutaEntrega entidad);
        RutaEntrega Modificar(RutaEntrega entidad);
        bool Borrar(int id);
    }
}
