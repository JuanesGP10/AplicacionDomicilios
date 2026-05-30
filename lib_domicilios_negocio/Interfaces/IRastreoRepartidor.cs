
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IRastreoRepartidor
    {
        List<RastreoRepartidor> Consultar();
        RastreoRepartidor ConsultarPorRepartidorId(int repartidorId);
        RastreoRepartidor ConsultarPorId(int id);
        RastreoRepartidor Guardar(RastreoRepartidor entidad);
        RastreoRepartidor Modificar(RastreoRepartidor entidad);
        bool Borrar(int id);
        bool ActualizarUbicacion(int id, decimal nLongitud, decimal nLatitud);
    }
}
