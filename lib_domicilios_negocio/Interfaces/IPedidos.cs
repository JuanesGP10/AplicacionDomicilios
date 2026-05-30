
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IPedidos
    {
        List<Pedidos> Consultar();
        Pedidos ConsultarPorId(int id);
        List<Pedidos> ConsultarPorCliente(int clienteId);
        Pedidos Guardar(Pedidos entidad);
        Pedidos Modificar(Pedidos entidad);
        bool Borrar(int id);
        bool ActualizarEstado(int id, int nuevoEstadoId);
    }
}
