
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface ICalificacionesPresentacion
    {
        Task<List<Calificaciones>> ConsultarAsync();
        Task<List<Calificaciones>> ConsultarPorPedidoIdAsync(int pedidoId);
        Task<Calificaciones> GuardarAsync(Calificaciones entidad);
        Task<Calificaciones> ModificarAsync(Calificaciones entidad);
        Task<bool> BorrarAsync(int id);
    }
}
