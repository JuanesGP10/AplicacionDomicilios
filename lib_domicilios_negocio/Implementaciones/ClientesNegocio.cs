using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;
using Microsoft.EntityFrameworkCore;

namespace lib_domicilios_negocio.Implementaciones
{
    public class ClientesNegocio : IClientes
    {
        private Conexion? iConexion;

        public List<Clientes> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Clientes!.ToList();
        }

        public Clientes ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var cliente = this.iConexion.Clientes!.Find(id);
            if (cliente == null)
                throw new Exception("El cliente no existe");

            return cliente;
        }

        public List<Clientes> ConsultarPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El nombre a buscar no puede estar vacío");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Clientes!
                .Where(u => u.Nombre != null && u.Nombre.Contains(nombre))
                .ToList();
        }

        public Clientes Guardar(Clientes entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Ya se guardó este cliente");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Clientes!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Clientes",
                Descripcion = $"Se registró un nuevo cliente en el sistema. Nombre: {entidad.Nombre}, Email: {entidad.Email}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Clientes Modificar(Clientes entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El cliente no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Clientes!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Clientes",
                Descripcion = $"Se modificaron los datos del cliente. Nombre: {entidad.Nombre}, Email: {entidad.Email}.",
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

            var usuario = this.iConexion.Clientes!.Find(id);
            if (usuario == null)
                throw new Exception("Cliente no encontrado para eliminar");

            this.iConexion.Clientes.Remove(usuario);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Clientes",
                Descripcion = $"Se eliminó al cliente del sistema. Nombre: {usuario.Nombre}, Email: {usuario.Email}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }

}

