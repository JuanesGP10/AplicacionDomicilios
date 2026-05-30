using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class CategoriasNegocio : ICategorias
    {
        private Conexion? iConexion;

        public List<Categorias> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Categorias!.ToList();
        }

        public Categorias Guardar(Categorias entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Esta categoría ya se encuentra registrada");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Categorias!.Add(entidad);
            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Categorias",
                Descripcion = $"Se registró una nueva categoría de productos. Nombre: {entidad.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Categorias Modificar(Categorias entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("La categoría no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Categorias!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Categorias",
                Descripcion = $"Se modifico la categoría con ID {entidad.Id}, Nombre: {entidad.Nombre}.",
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

            var categoria = this.iConexion.Categorias!.Find(id);
            if (categoria == null)
                throw new Exception("Categoría no encontrada para eliminar");

            this.iConexion.Categorias.Remove(categoria);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Categorias",
                Descripcion = $"Se eliminó del sistema la categoría con ID {id}, Nombre: {categoria.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
