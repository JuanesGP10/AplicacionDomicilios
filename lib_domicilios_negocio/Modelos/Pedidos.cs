
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace lib_domicilios_negocio.Modelos
{
    public class Pedidos
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int RepartidorId { get; set; }
        public int EstadoPedidoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }

        [ForeignKey("ClienteId")] public Clientes? _ClienteId;
        [ForeignKey("RepartidorId")] public Repartidores? _RepartidorId { get; set; }
        [ForeignKey("EstadoPedidoId")] public EstadoPedido? _EstadoPedidoId { get; set; }
        [NotMapped] public List<DetallePedido>? DetallePedido { get; set; }
        [NotMapped] public List<Pagos>? Pagos { get; set; }
        [NotMapped] public List<Facturas>? Facturas { get; set; }
        [NotMapped] public Calificaciones? Calificaciones { get; set; }
        [NotMapped] public List<RutaEntrega>? RutaEntrega { get; set; }
        [NotMapped] public List<RastreoPedido>? RastreoPedido { get; set; }
    }
}
