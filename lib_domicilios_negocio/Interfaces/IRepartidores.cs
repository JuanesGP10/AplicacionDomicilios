using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IRepartidores
    {
        List<Repartidores> Consultar();
        Repartidores ConsultarPorId(int id);
        List<Repartidores> ConsultarPorNombre(string nombre);
        Repartidores Guardar(Repartidores entidad);
        Repartidores Modificar(Repartidores entidad);
        bool Borrar(int id);
    }
}