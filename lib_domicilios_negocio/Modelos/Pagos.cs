
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Pagos
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int MetodoPagoId { get; set; }
        public int EstadoPagoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Comision { get; set; }

        [ForeignKey("PedidoId")] public Pedidos? _PedidoId { get; set; }
        [ForeignKey("MetodoPagoId")] public MetodoPago? _MetodoPagoId { get; set; }
        [ForeignKey("EstadoPagoId")] public EstadoPago? _EstadoPagoId { get; set; }
        [NotMapped] public List<Facturas>? Facturas { get; set; }
    }
}
