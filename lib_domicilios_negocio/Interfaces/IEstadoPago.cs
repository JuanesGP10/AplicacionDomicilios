
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IEstadoPago
    {
        List<EstadoPago> Consultar();
        EstadoPago Guardar(EstadoPago entidad);
        EstadoPago Modificar(EstadoPago entidad);
        bool Borrar(int id);
    }
}
