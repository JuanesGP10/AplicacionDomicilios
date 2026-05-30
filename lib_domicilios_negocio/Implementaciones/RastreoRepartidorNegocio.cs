
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class RastreoRepartidorNegocio : IRastreoRepartidor
    {
        private Conexion? iConexion;

        public List<RastreoRepartidor> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.RastreoRepartidor!.ToList();
        }

        public RastreoRepartidor ConsultarPorRepartidorId(int repartidorId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var rastreo = this.iConexion.RastreoRepartidor!
                .FirstOrDefault(r => r.RepartidorId == repartidorId);

            if (rastreo == null)
                throw new Exception("El repartidor especificado no cuenta con un registro de rastreo activo actualmente.");

            return rastreo;
        }

        public RastreoRepartidor ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var rastreo = this.iConexion.RastreoRepartidor!.Find(id);
            if (rastreo == null)
                throw new Exception("El rastreo no existe");

            return rastreo;
        }

        public RastreoRepartidor Guardar(RastreoRepartidor entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este registro de rastreo ya se encuentra asentado en el sistema");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.RastreoRepartidor!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoRepartidor",
                Descripcion = $"Se registró un nuevo de rastreo para el repartidor con ID {entidad.RepartidorId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public RastreoRepartidor Modificar(RastreoRepartidor entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El registro de rastreo no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.RastreoRepartidor!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoRepartidor",
                Descripcion = $"Se modificó el rastreo del repartidor con ID {entidad.RepartidorId}.",
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

            var rastreo = this.iConexion.RastreoRepartidor!.Find(id);
            if (rastreo == null)
                throw new Exception("Registro de rastreo no encontrado para eliminar");

            this.iConexion.RastreoRepartidor.Remove(rastreo);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoRepartidor",
                Descripcion = $"Se eliminó del sistema el rastreo con ID {id} correspondiente al repartidor ID {rastreo.RepartidorId}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
        public bool ActualizarUbicacion(int id, decimal nLongitud, decimal nLatitud)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var rastreo = this.iConexion.RastreoRepartidor!.Find(id);
            if (rastreo == null)
                throw new Exception("No se encontró la ruta de entrega para actualizar la ubicación");

            rastreo.Latitud = nLatitud;
            rastreo.Longitud = nLongitud;
            rastreo.FechaActualizacion = DateTime.Now;

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "RastreoRepartidor",
                Descripcion = $"Actualizacion de ubicacion del ratreo con ID {id}, Lat: {nLatitud}, Long: {nLongitud}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}

