
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IVehiculos
    {
        List<Vehiculos> Consultar();
        Vehiculos ConsultarPorPlaca(string placa);
        Vehiculos Guardar(Vehiculos entidad);
        Vehiculos Modificar(Vehiculos entidad);
        bool Borrar(int id);
    }
}
