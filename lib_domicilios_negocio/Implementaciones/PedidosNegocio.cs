

using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class PedidosNegocio : IPedidos
    {
        private Conexion? iConexion;

        public List<Pedidos> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Pedidos!.ToList();
        }

        public Pedidos ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var pedido = this.iConexion.Pedidos!.Find(id);
            if (pedido == null)
                throw new Exception("El pedido solicitado no existe");

            return pedido;
        }

        public List<Pedidos> ConsultarPorCliente(int clienteId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Pedidos!
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.Id)
                .ToList();
        }
        public Pedidos Guardar(Pedidos entidad)
        {

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Pedidos!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pedidos",
                Descripcion = $"Se registró un nuevo pedido. Total: {entidad.Total}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Pedidos Modificar(Pedidos entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("No se puede modificar un pedido sin un ID válido");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Pedidos!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pedidos",
                Descripcion = $"Se actualizaron los montos del pedido con ID {entidad.Id}. Nuevo Total: {entidad.Total}.",
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

            var pedido = this.iConexion.Pedidos!.Find(id);
            if (pedido == null)
                throw new Exception("Pedido no encontrado para eliminar");

            this.iConexion.Pedidos.Remove(pedido);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pedidos",
                Descripcion = $"Se eliminó del sistema el pedido con ID {id}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
        public bool ActualizarEstado(int id, int nuevoEstadoId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var pedido = this.iConexion.Pedidos!.Find(id);
            if (pedido == null)
                throw new Exception("El pedido solicitado no existe");

            var nEstado = this.iConexion.EstadoPedido!.Find(nuevoEstadoId);
            if (nEstado == null)
                throw new Exception("El estado solicitado no existe");

            pedido.EstadoPedidoId = nuevoEstadoId;

            if (nEstado.Notificar)
            {
                Notificaciones notificacion = new Notificaciones
                {
                    UsuarioId = pedido.ClienteId,
                    Mensaje = "Amig@ tu pedido ahora está en estado: " + nEstado.Nombre,
                    FechaEnvio = DateTime.Now,
                    Leida = false
                };

                this.iConexion.Notificaciones!.Add(notificacion);
            }

            if (nEstado.Nombre == "Completado")
            {
                var repartidor = this.iConexion.Repartidores!.Find(pedido.RepartidorId);
                if (repartidor != null)
                {
                    decimal suma = 0;
                    int contador = 0;

                    foreach (var p in repartidor.Pedidos)
                    {
                        if (p._EstadoPedidoId.Nombre == "Completado" && p.Calificaciones != null)
                        {
                            suma += p.Calificaciones.Puntaje;
                            contador++;
                        }
                    }
                    repartidor.CalificacionPromedio = contador > 0 ? (suma / contador) : 0m;
                    this.iConexion.Repartidores.Update(repartidor);
                }
            }

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pedidos",
                Descripcion = $"Se cambió el estado del pedido ID {id} a '{nEstado.Nombre}'.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
