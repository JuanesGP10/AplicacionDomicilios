
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class FacturasNegocio : IFacturas
    {
        private Conexion? iConexion;

        public List<Facturas> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Facturas!.ToList();
        }

        public List<Facturas> ConsultarPorPedidoId(int pedidoId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Facturas!
                .Where(f => f.PedidoId == pedidoId)
                .ToList();
        }

        public Facturas ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var factura = this.iConexion.Facturas!.Find(id);
            if (factura == null)
                throw new Exception("La factura no existe");

            return factura;
        }

        public Facturas Guardar(Facturas entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Esta factura ya se encuentra registrada en el sistema");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Facturas!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Facturas",
                Descripcion = $"Se generó una nueva factura para el pedido ID {entidad.PedidoId}, Total: {entidad.Total}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Facturas Modificar(Facturas entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("La factura no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Facturas!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Facturas",
                Descripcion = $"Se modificaron los montos de la factura con ID {entidad.Id} (Pedido ID: {entidad.PedidoId}). Nuevo Total: {entidad.Total}.",
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

            var factura = this.iConexion.Facturas!.Find(id);
            if (factura == null)
                throw new Exception("Factura no encontrada para eliminar");

            this.iConexion.Facturas.Remove(factura);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Facturas",
                Descripcion = $"Se eliminó del sistema la factura con ID {id} que pertenecía al pedido ID {factura.PedidoId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}