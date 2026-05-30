
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class RastreoPedidoNegocio : IRastreoPedido
    {
        private Conexion? iConexion;

        public List<RastreoPedido> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.RastreoPedido!.ToList();
        }

        public List<RastreoPedido> ConsultarPorPedidoId(int pedidoId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.RastreoPedido!
                .Where(r => r.PedidoId == pedidoId)
                .OrderBy(r => r.FechaActualizacion)
                .ToList();
        }

        public RastreoPedido ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var rastreo = this.iConexion.RastreoPedido!.Find(id);
            if (rastreo == null)
                throw new Exception("El rastreo no existe");

            return rastreo;
        }
        public RastreoPedido Guardar(RastreoPedido entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este registro de rastreo ya se encuentra asentado en el sistema");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.RastreoPedido!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoPedido",
                Descripcion = $"Se registró un nuevo de rastreo para el pedido ID {entidad.PedidoId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public RastreoPedido Modificar(RastreoPedido entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El registro de rastreo no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.RastreoPedido!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoPedido",
                Descripcion = $"Se modificó el hito de rastreo ID {entidad.Id} del pedido ID {entidad.PedidoId}.",
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

            var rastreo = this.iConexion.RastreoPedido!.Find(id);
            if (rastreo == null)
                throw new Exception("Registro de rastreo no encontrado para eliminar");

            this.iConexion.RastreoPedido.Remove(rastreo);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoPedido",
                Descripcion = $"Se eliminó del sistema el hito de rastreo con ID {id} correspondiente al pedido ID {rastreo.PedidoId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
        public bool ActualizarUbicacion(int id, decimal nLongitud, decimal nLatitud)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var rastreo = this.iConexion.RastreoPedido!.Find(id);
            if (rastreo == null)
                throw new Exception("No se encontró la ruta de entrega para actualizar la ubicación");

            rastreo.Latitud = nLatitud;
            rastreo.Longitud = nLongitud;
            rastreo.FechaActualizacion = DateTime.Now;

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoPedido",
                Descripcion = $"Actualizacion de ubicacion del pedido con ID {id}, Lat: {nLatitud}, Long: {nLongitud}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
