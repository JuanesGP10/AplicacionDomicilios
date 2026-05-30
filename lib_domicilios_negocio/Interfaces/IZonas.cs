
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IZonas
    {
        List<Zonas> Consultar();
        Zonas Guardar(Zonas entidad);
        Zonas Modificar(Zonas entidad);
        bool Borrar(int id);
    }
}
    

