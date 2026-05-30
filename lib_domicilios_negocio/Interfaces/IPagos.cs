using lib_domicilios_negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IPagos
    {
        List<Pagos> Consultar();
        List<Pagos> ConsultarPorPedidoId(int pedidoId);
        Pagos ConsultarPorId(int id);
        Pagos Guardar(Pagos entidad);
        Pagos Modificar(Pagos entidad);
        bool Borrar(int id);
    }
}
