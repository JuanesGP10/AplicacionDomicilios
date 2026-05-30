
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface INotificaciones
    {
        List<Notificaciones> Consultar();
        List<Notificaciones> ConsultarPorUsuarioId(int usuarioId);
        Notificaciones ConsultarPorId(int id);
        Notificaciones Guardar(Notificaciones entidad);
        Notificaciones Modificar(Notificaciones entidad);
        bool Borrar(int id);
    }
}
