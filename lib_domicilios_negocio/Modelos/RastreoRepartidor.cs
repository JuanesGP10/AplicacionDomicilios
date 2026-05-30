
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class RastreoRepartidor
    {
        public int Id { get; set; }
        public int RepartidorId { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public DateTime FechaActualizacion { get; set; }

        [ForeignKey("RepartidorId")] public Repartidores? _RepartidorId { get; set; }
    }
}
