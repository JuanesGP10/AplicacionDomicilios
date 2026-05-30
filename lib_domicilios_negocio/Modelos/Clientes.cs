
using System.ComponentModel.DataAnnotations.Schema;


namespace lib_domicilios_negocio.Modelos
{
    public class Clientes : Usuarios
    {
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int MetodoPagoFav { get; set; }
        public bool Activo { get; set; }

        [ForeignKey("MetodoPagoFav")]public MetodoPago? _MetodoPagoFav { get; set; }
        [NotMapped]public List<Pedidos>? Pedidos { get; set; }
    }
}
