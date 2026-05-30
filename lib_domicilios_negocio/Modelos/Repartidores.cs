
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Repartidores : Usuarios
    {
        public int VehiculoId { get; set; }
        public int ZonaId { get; set; }
        public bool Disponible { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public bool Activo { get; set; }

        [ForeignKey("VehiculoId")] public Vehiculos? _VehiculoId { get; set; }
        [ForeignKey("ZonaId")] public Zonas? _ZonaId { get; set; }
        [NotMapped]public List<Pedidos>? Pedidos { get; set; }
        [NotMapped] public List<RastreoRepartidor>? RastreoRepartidor { get; set; }
    }
}
