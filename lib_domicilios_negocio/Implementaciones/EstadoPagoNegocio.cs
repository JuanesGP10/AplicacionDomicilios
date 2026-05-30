
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class EstadoPagoNegocio : IEstadoPago
    {
        private Conexion? iConexion;

        public List<EstadoPago> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.EstadoPago!.ToList();
        }

        public EstadoPago Guardar(EstadoPago entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este estado de pago ya se encuentra registrado");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.EstadoPago!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "EstadoPago",
                Descripcion = $"Se creó un nuevo estado de pago. Nombre: {entidad.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public EstadoPago Modificar(EstadoPago entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El estado de pago no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.EstadoPago!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "EstadoPago",
                Descripcion = $"Se modificaron las propiedades del estado de pago con ID {entidad.Id}, Nombre: {entidad.Nombre}.",
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

            var estado = this.iConexion.EstadoPago!.Find(id);
            if (estado == null)
                throw new Exception("Estado de pago no encontrado para eliminar");

            this.iConexion.EstadoPago.Remove(estado);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "EstadoPago",
                Descripcion = $"Se eliminó del sistema el estado de pago con ID {id}, Nombre: {estado.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
