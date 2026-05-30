
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class RutaEntrega
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public decimal DistanciaKM { get; set; }
        public decimal TiempoEstimado { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("PedidoId")] public Pedidos? _PedidoId { get; set; }
    }
}
