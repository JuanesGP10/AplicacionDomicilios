
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Zonas
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Tarifa { get; set; }

        [NotMapped]public List<Repartidores>? Repartidores { get; set; }
    }

}
