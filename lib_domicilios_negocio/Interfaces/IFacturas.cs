
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IFacturas
    {
        List<Facturas> Consultar();
        List<Facturas> ConsultarPorPedidoId(int pedidoId);
        Facturas ConsultarPorId(int id);
        Facturas Guardar(Facturas entidad);
        Facturas Modificar(Facturas entidad);
        bool Borrar(int id);
    }
}
