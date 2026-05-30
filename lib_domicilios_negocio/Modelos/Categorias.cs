
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Categorias
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Creacion { get; set; }
        public bool Activo { get; set; }
        [NotMapped]public List<Productos>? Productos { get; set; }
    }
    
}
