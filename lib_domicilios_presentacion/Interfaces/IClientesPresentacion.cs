
using lib_domicilios_negocio.Modelos;

namespace lib_domicilios_presentacion.Interfaces
{
    public interface IClientesPresentacion
    {
        Task<List<Clientes>> ConsultarAsync();
        Task<Clientes> ConsultarPorIdAsync(int id);
        Task<Clientes> ConsultarPorNombreAsync(string nombre);
        Task<Clientes> GuardarAsync(Clientes entidad);
        Task<Clientes> ModificarAsync(Clientes entidad);
        Task<bool> BorrarAsync(int id);
    }
}
