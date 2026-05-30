
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Historicos
    {
        public int Id { get; set; }
        public string? EntidadAfectada { get; set; }
        public String? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
