
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface ICalificaciones
    {
        List<Calificaciones> Consultar();
        List<Calificaciones> ConsultarPorPedidoId(int pedidoId);
        Calificaciones Guardar(Calificaciones entidad);
        Calificaciones Modificar(Calificaciones entidad);
        bool Borrar(int id);
    }
}
