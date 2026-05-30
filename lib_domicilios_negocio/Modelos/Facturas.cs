
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Facturas
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int PagoId { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }

        [ForeignKey("PedidoId")] public Pedidos? _PedidoId { get; set; }
        [ForeignKey("PagoId")] public Pagos? _PagoId { get; set; }
    }
}
