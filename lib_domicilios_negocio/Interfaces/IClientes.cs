using lib_domicilios_negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IClientes
    {
        List<Clientes> Consultar();
        Clientes ConsultarPorId(int id);
        List<Clientes> ConsultarPorNombre(string nombre);
        Clientes Guardar(Clientes entidad);
        Clientes Modificar(Clientes entidad);
        bool Borrar(int id);
    }
}