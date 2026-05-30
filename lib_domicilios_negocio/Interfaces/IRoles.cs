
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IRoles
    {
        List<Roles> Consultar();
        Roles ConsultarPorNombre(string nombre);
    }
}
