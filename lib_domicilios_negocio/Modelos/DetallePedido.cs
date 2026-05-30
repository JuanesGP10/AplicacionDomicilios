
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }


        [ForeignKey("PedidoId")] public Pedidos? _PedidoId { get; set; }
        [ForeignKey("ProductoId")] public Productos? _ProductoId { get; set; }

    }
}
