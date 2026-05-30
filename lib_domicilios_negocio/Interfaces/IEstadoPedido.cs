using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IEstadoPedido
    {
        List<EstadoPedido> Consultar();
        EstadoPedido Guardar(EstadoPedido entidad);
        EstadoPedido Modificar(EstadoPedido entidad);
        bool Borrar(int id);
    }
}
