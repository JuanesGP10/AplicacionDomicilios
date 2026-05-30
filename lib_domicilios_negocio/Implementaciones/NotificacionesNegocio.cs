
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class NotificacionesNegocio : INotificaciones
    {
        private Conexion? iConexion;

        public List<Notificaciones> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Notificaciones!.ToList();
        }

        public List<Notificaciones> ConsultarPorUsuarioId(int usuarioId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Notificaciones!
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaEnvio)
                .ToList();
        }

        public Notificaciones ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var notificacion = this.iConexion.Notificaciones!.Find(id);
            if (notificacion == null)
                throw new Exception("La notificacion no existe");

            return notificacion;
        }

        public Notificaciones Guardar(Notificaciones entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Esta notificación ya se encuentra registrada en el sistema");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Notificaciones!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Notificaciones",
                Descripcion = $"Se generó una notificación para el usuario ID {entidad.UsuarioId}, Mensaje: {entidad.Mensaje}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Notificaciones Modificar(Notificaciones entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("La notificación no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Notificaciones!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Notificaciones",
                Descripcion = $"Se modificó el estado o contenido de la notificación ID {entidad.Id}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public bool Borrar(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var notificacion = this.iConexion.Notificaciones!.Find(id);
            if (notificacion == null)
                throw new Exception("Notificación no encontrada para eliminar");

            this.iConexion.Notificaciones.Remove(notificacion);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Notificaciones",
                Descripcion = $"Se eliminó del sistema la notificación con ID {id} dirigida al usuario ID {notificacion.UsuarioId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
