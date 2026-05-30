
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class EstadoPago
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public bool Activo { get; set; }

        [NotMapped]public List<Pagos>? Pagos { get; set; }
    }
}
