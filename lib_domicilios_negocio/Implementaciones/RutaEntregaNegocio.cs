
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class RutaEntregaNegocio : IRutaEntrega
    {
        private Conexion? iConexion;

        public List<RutaEntrega> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.RutaEntrega!.ToList();
        }

        public List<RutaEntrega> ConsultarPorPedidoId(int pedidoId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.RutaEntrega!
                .Where(r => r.PedidoId == pedidoId)
                .ToList();
        }

        public RutaEntrega Guardar(RutaEntrega entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Esta ruta de entrega ya se encuentra registrada en el sistema");

            decimal velocidadPromedio = 30m;
            entidad.TiempoEstimado = (entidad.DistanciaKM / velocidadPromedio) * 100;
    

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.RutaEntrega!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RutaEntrega",
                Descripcion = $"Se guardo una nueva ruta para pedido ID {entidad.PedidoId}, Distancia: {entidad.DistanciaKM} KM, Tiempo Estimado: {entidad.TiempoEstimado}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public RutaEntrega Modificar(RutaEntrega entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("La ruta de entrega no se puede modificar porque no existe en la base de datos");

            decimal velocidadPromedio = 30m;
            entidad.TiempoEstimado = (entidad.DistanciaKM / velocidadPromedio) * 100;

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.RutaEntrega!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RutaEntrega",
                Descripcion = $"Se modificó la ruta ID {entidad.Id} (Pedido ID: {entidad.PedidoId}). Nuevo Tiempo Estimado: {entidad.TiempoEstimado}.",
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

            var ruta = this.iConexion.RutaEntrega!.Find(id);
            if (ruta == null)
                throw new Exception("Ruta de entrega no encontrado para eliminar");

            this.iConexion.RutaEntrega.Remove(ruta);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RutaEntrega",
                Descripcion = $"Se eliminó del sistema la ruta de entrega logística con ID {id}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
