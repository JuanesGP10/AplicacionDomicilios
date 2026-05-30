

using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class MetodoPagoNegocio : IMetodoPago
    {
        private Conexion? iConexion;

        public List<MetodoPago> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.MetodoPago!.ToList();
        }

        public MetodoPago ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var metodo = this.iConexion.MetodoPago!.Find(id);
            if (metodo == null)
                throw new Exception("El pedido solicitado no existe");

            return metodo;
        }

        public MetodoPago Guardar(MetodoPago entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este método de pago ya se encuentra registrado");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.MetodoPago!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "MetodoPago",
                Descripcion = $"Se registró un nuevo método de pago. Nombre: {entidad.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public MetodoPago Modificar(MetodoPago entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El método de pago no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.MetodoPago!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "MetodoPago",
                Descripcion = $"Se modificaron las propiedades del método de pago con ID {entidad.Id}. Nombre: {entidad.Nombre}.",
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

            var metodo = this.iConexion.MetodoPago!.Find(id);
            if (metodo == null)
                throw new Exception("Método de pago no encontrado para eliminar");

            this.iConexion.MetodoPago.Remove(metodo);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "MetodoPago",
                Descripcion = $"Se eliminó del sistema el método de pago con ID {id} (Nombre: {metodo.Nombre}).",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
