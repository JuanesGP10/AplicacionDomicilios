

using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class DetallePedidoNegocio : IDetallePedido
    {
        private Conexion? iConexion;

        public List<DetallePedido> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.DetallePedido!.ToList();
        }

        public List<DetallePedido> ConsultarPorPedidoId(int pedidoId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.DetallePedido!
                .Where(d => d.PedidoId == pedidoId)
                .ToList();
        }

        public DetallePedido Guardar(DetallePedido entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este detalle de pedido ya se encuentra registrado");

            entidad.Subtotal = entidad.Cantidad * entidad.PrecioUnitario;

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.DetallePedido!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "DetallePedido",
                Descripcion = $"Se agregó un producto al pedido ID {entidad.PedidoId} (Cant: {entidad.Cantidad}, Subtotal: {entidad.Subtotal}).",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public DetallePedido Modificar(DetallePedido entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El detalle no se puede modificar porque no existe en la base de datos");

            // Recalculamos el subtotal por si modificaron la cantidad en el panel
            entidad.Subtotal = entidad.Cantidad * entidad.PrecioUnitario;

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            // 1. Actualizar el registro
            this.iConexion.DetallePedido!.Update(entidad);

            // 2. Crear auditoría interna
            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "DetallePedido",
                Descripcion = $"Se modificó el detalle ID {entidad.Id} del pedido ID {entidad.PedidoId}. Nuevo Subtotal: {entidad.Subtotal}.",
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

            var detalle = this.iConexion.DetallePedido!.Find(id);
            if (detalle == null)
                throw new Exception("Detalle de pedido no encontrado para eliminar");

            // 1. Remover el ítem
            this.iConexion.DetallePedido.Remove(detalle);

            // 2. Crear auditoría interna
            Historicos auditoria = new Historicos
            {
                EntidadAfectada ="DetallePedido",
                Descripcion = $"Se eliminó el detalle ID {id} del pedido ID {detalle.PedidoId} (Se removió el producto del carrito).",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}