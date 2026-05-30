
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Productos
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Urlimagen { get; set; }
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")] public Categorias? _CategoriaId { get; set; }
        [NotMapped]public List<DetallePedido>? DetallePedido { get; set; }
    }
}
