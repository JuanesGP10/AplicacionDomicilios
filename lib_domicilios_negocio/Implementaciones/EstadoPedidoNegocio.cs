

using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class EstadoPedidoNegocio
    {
        private Conexion? iConexion;

        public List<EstadoPedido> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.EstadoPedido!.ToList();
        }

        public EstadoPedido ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var estado = this.iConexion.EstadoPedido!.Find(id);
            if (estado == null)
                throw new Exception("El estado de pedido solicitado no existe");

            return estado;
        }

        public EstadoPedido Guardar(EstadoPedido entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este estado de pedido ya se encuentra registrado");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.EstadoPedido!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pedidos",
                Descripcion = $"Se creó un nuevo estado de pedido. Nombre: {entidad.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public EstadoPedido Modificar(EstadoPedido entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El estado no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.EstadoPedido!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pedidos",
                Descripcion = $"Se modificaron las propiedades del estado de pedido con ID {entidad.Id}, Nombre: {entidad.Nombre}.",
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

            var estado = this.iConexion.EstadoPedido!.Find(id);
            if (estado == null)
                throw new Exception("Estado de pedido no encontrado para eliminar");

            this.iConexion.EstadoPedido.Remove(estado);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Pedidos",
                Descripcion = $"Se eliminó del sistema el estado de pedido con ID {id} (Nombre: {estado.Nombre}).",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
    

