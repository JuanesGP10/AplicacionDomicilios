

using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Calificaciones
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int Puntaje { get; set; }
        public string? Comentario { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("PedidoId")]public Pedidos? _PedidoId { get; set; }
    }
}
