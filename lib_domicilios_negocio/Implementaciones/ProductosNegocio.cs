using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_domicilios_negocio.Implementaciones
{
    public class ProductosNegocio : IProductos
    {
        private Conexion? iConexion;

        public List<Productos> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Productos!.ToList();
        }

 
        public List<Productos> ConsultarPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El nombre a buscar no puede estar vacío");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Productos!
                .Where(u => u.Nombre != null && u.Nombre.Contains(nombre))
                .ToList();
        }

        public List<Productos> ConsultarPorCategoria(int categoriaId)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Productos!
                .Where(p => p.CategoriaId == categoriaId)
                .ToList();
        }

        public Productos Guardar(Productos entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Este producto ya se encuentra registrado en el catálogo");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Productos!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Productos",
                Descripcion = $"Se integró un nuevo producto. Nombre: {entidad.Nombre}, Precio: {entidad.Precio}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Productos Modificar(Productos entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El producto no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Productos!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Productos",
                Descripcion = $"Se modifico el producto con ID {entidad.Id}, Nombre: {entidad.Nombre}, Precio: {entidad.Precio}.",
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

            var producto = this.iConexion.Productos!.Find(id);
            if (producto == null)
                throw new Exception("Producto no encontrado para eliminar");

            this.iConexion.Productos.Remove(producto);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Productos",
                Descripcion = $"Se eliminó del catálogo el producto con ID {id}, Nombre: {producto.Nombre}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
