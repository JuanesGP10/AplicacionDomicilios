
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface INotificacionesPresentacion
    {
        Task<List<Notificaciones>> ConsultarAsync();
        Task<List<Notificaciones>> ConsultarPorUsuarioIdAsync(int usuarioId);
        Task<Notificaciones> ConsultarPorIdAsync(int id);
        Task<Notificaciones> GuardarAsync(Notificaciones entidad);
        Task<Notificaciones> ModificarAsync(Notificaciones entidad);
        Task<bool> BorrarAsync(int id);
    }
}
