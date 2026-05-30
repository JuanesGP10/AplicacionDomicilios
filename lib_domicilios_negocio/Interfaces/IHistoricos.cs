using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IHistoricos
    {
        List<Historicos> Consultar();
        Historicos Guardar(Historicos entidad);
    }
}
