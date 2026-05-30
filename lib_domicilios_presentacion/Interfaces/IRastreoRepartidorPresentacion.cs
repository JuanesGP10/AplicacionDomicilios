
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IRastreoRepartidorPresentacion
    {
        Task<List<RastreoRepartidor>> ConsultarAsync();
        Task<RastreoRepartidor> ConsultarPorRepartidorIdAsync(int repartidorId);
        Task<RastreoRepartidor> ConsultarPorIdAsync(int id);
        Task<RastreoRepartidor> GuardarAsync(RastreoRepartidor entidad);
        Task<RastreoRepartidor> ModificarAsync(RastreoRepartidor entidad);
        Task<bool> BorrarAsync(int id);
        Task<bool> ActualizarUbicacionAsync(int id, decimal nLongitud, decimal nLatitud);
    }
}
