
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class MetodoPago
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Comision { get; set; }
        public bool Activo { get; set; }

        [NotMapped]public List<Clientes>? Clientes { get; set; }
        [NotMapped] public List<Pagos>? Pagos { get; set; }
    }
}
