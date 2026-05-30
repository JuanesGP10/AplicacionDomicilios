
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class UsuariosNegocio : IUsuarios
    {
        private Conexion? iConexion;

        public List<Usuarios> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Usuarios!.ToList();
        }

        public Usuarios ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var usuario = this.iConexion.Usuarios!.Find(id);
            if (usuario == null)
                throw new Exception("El usuario no existe");

            return usuario;
        }

        public List<Usuarios> ConsultarPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El nombre a buscar no puede estar vacío");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Usuarios!
                .Where(u => u.Nombre != null && u.Nombre.Contains(nombre))
                .ToList();
        }

        public Usuarios Guardar(Usuarios entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Ya se guardó este usuario");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Usuarios!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Usuarios",
                Descripcion = $"Se registró un nuevo usuario en el sistema. Nombre: {entidad.Nombre}, Email: {entidad.Email}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Usuarios Modificar(Usuarios entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El usuario no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Usuarios!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Usuarios",
                Descripcion = $"Se modificaron los datos del usuario. Nombre: {entidad.Nombre}, Email: {entidad.Email}.",
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

            var usuario = this.iConexion.Usuarios!.Find(id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado para eliminar");

            this.iConexion.Usuarios.Remove(usuario);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Usuarios",
                Descripcion = $"Se eliminó al usuario del sistema. Nombre: {usuario.Nombre}, Email: {usuario.Email}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
