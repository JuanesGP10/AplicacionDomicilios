
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class ZonasNegocio : IZonas
    {
        private Conexion? iConexion;

        public List<Zonas> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Zonas!.ToList();
        }

        public Zonas Guardar(Zonas entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Ya se guardó esta zona");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Zonas!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Zonas",
                Descripcion = $"Se registró una nueva zona de cobertura. Nombre: {entidad.Nombre}, Tarifa: {entidad.Tarifa}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Zonas Modificar(Zonas entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("La zona no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Zonas!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Zonas",
                Descripcion = $"Se modifico la zona con ID {entidad.Id}, Nombre: {entidad.Nombre}.",
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

            var zona = this.iConexion.Zonas!.Find(id);
            if (zona == null)
                throw new Exception("Zona no encontrada para eliminar");

            this.iConexion.Zonas.Remove(zona);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Zonas",
                Descripcion = $"Se eliminó del sistema la zona de cobertura con ID {id}, Nombre: {zona.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
    

