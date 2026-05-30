
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IHistoricosPresentacion
    {
        Task<List<Historicos>> ConsultarAsync();
        Task<Historicos> GuardarAsync(Historicos entidad);
    }
}
