
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_domicilios_negocio.Modelos
{
    public class Notificaciones
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Leida { get; set; }

        [ForeignKey("UsuarioId")] public Usuarios? _UsuarioId { get; set; }
    }
}
