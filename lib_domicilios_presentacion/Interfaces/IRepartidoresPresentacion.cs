
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IRepartidoresPresentacion
    {
        Task<List<Repartidores>> ConsultarAsync();
        Task<Repartidores> ConsultarPorIdAsync(int id);
        Task<Repartidores> ConsultarPorNombreAsync(string nombre);
        Task<Repartidores> GuardarAsync(Repartidores entidad);
        Task<Repartidores> ModificarAsync(Repartidores entidad);
        Task<bool> BorrarAsync(int id);
    }
}
