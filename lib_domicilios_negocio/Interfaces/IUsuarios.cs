
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IUsuarios
    {
        List<Usuarios> Consultar();
        Usuarios ConsultarPorId(int id);
        List<Usuarios> ConsultarPorNombre(string nombre);          
        Usuarios Guardar(Usuarios entidad);
        Usuarios Modificar(Usuarios entidad);
        bool Borrar(int id);
    }
}
