

using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class PagosNegocio : IPagos
    {
        private Conexion? iConexion;

        public List<Pagos> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Pagos!.ToList();
        }

        public List<Pagos> ConsultarPorPedidoId(int pedidoId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Pagos!
                .Where(p => p.PedidoId == pedidoId)
                .ToList();
        }

        public Pagos ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var pago = this.iConexion.Pagos!.Find(id);
            if (pago == null)
                throw new Exception("La factura no existe");

            return pago;
        }

        public Pagos Guardar(Pagos entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este pago ya se encuentra registrado en el sistema");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Pagos!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pagos",
                Descripcion = $"Se registró un nuevo pago para el pedido ID {entidad.PedidoId}, Monto: {entidad.Monto}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Pagos Modificar(Pagos entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El registro de pago no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Pagos!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pagos",
                Descripcion = $"Se modificó el pago con ID {entidad.Id} asociado al pedido ID {entidad.PedidoId}. Nuevo Monto: {entidad.Monto}.",
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

            var pago = this.iConexion.Pagos!.Find(id);
            if (pago == null)
                throw new Exception("Registro de pago no encontrado para eliminar");

            this.iConexion.Pagos.Remove(pago);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pagos",
                Descripcion = $"Se eliminó el registro de pago con ID {id} que correspondía al pedido ID {pago.PedidoId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
