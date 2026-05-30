
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IMetodoPago
    {
        List<MetodoPago> Consultar();
        MetodoPago ConsultarPorId(int id);
        MetodoPago Guardar(MetodoPago entidad);
        MetodoPago Modificar(MetodoPago entidad);
        bool Borrar(int id);
    }
}
