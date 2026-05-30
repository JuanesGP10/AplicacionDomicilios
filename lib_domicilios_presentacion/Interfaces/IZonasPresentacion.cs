
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IZonasPresentacion
    {
        Task<List<Zonas>> ConsultarAsync();
        Task<Zonas> GuardarAsync(Zonas entidad);
        Task<Zonas> ModificarAsync(Zonas entidad);
        Task<bool> BorrarAsync(int id);
    }
}
