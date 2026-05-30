
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class CalificacionesNegocio : ICalificaciones
    {
        private Conexion? iConexion;

        public List<Calificaciones> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Calificaciones!.ToList();
        }

        public List<Calificaciones> ConsultarPorPedidoId(int pedidoId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Calificaciones!
                .Where(c => c.PedidoId == pedidoId)
                .ToList();
        }

        public Calificaciones Guardar(Calificaciones entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Esta calificación ya se encuentra registrada");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Calificaciones!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Calificaciones",
                Descripcion = $"Se registró una nueva calificación para el pedido ID {entidad.PedidoId}, Puntaje: {entidad.Puntaje}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Calificaciones Modificar(Calificaciones entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("La calificación no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Calificaciones!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Calificaciones",
                Descripcion = $"Se modificó la calificación con ID {entidad.Id}, Pedido ID: {entidad.PedidoId}), Puntaje: {entidad.Puntaje}.",
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

            var calificacion = this.iConexion.Calificaciones!.Find(id);
            if (calificacion == null)
                throw new Exception("Calificación no encontrada para eliminar");

            this.iConexion.Calificaciones.Remove(calificacion);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Calificaciones",
                Descripcion = $"Se eliminó del sistema la calificación con ID {id} vinculada al pedido ID {calificacion.PedidoId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
