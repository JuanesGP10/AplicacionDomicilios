
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IVehiculosPresentacion
    {
        Task<List<Vehiculos>> ConsultarAsync();
        Task<Vehiculos> ConsultarPorPlacaAsync(string placa);
        Task<Vehiculos> GuardarAsync(Vehiculos entidad);
        Task<Vehiculos> ModificarAsync(Vehiculos entidad);
        Task<bool> BorrarAsync(int id);
    }
}
