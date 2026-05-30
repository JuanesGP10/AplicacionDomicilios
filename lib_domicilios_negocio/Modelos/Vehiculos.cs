
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Vehiculos
    {
        public int Id { get; set; }
        public string? Placa { get; set; }
        public string? Tipo { get; set; }
        public string? Modelo { get; set; }
        public bool Activo { get; set; }

        [NotMapped]public List<Repartidores>? Repartidores { get; set; }
    }
}
