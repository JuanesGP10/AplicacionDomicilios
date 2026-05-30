
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class RastreoPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime FechaActualizacion { get; set; }

        [ForeignKey("PedidoId")] public Pedidos? _PedidoId { get; set; }
    }
}
