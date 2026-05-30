using lib_domicilios_negocio.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IDetallePedido
    {
        List<DetallePedido> Consultar();
        List<DetallePedido> ConsultarPorPedidoId(int pedidoId);
        DetallePedido Guardar(DetallePedido entidad);
        DetallePedido Modificar(DetallePedido entidad);
        bool Borrar(int id);
    }
}
