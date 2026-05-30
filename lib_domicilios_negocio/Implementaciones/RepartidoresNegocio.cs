
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class RepartidoresNegocio : IRepartidores
    {
        private Conexion? iConexion;

        public List<Repartidores> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Repartidores!.ToList();
        }

        public Repartidores ConsultarPorId(int id)
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var usuario = this.iConexion.Repartidores!.Find(id);
            if (usuario == null)
                throw new Exception("El repartidor no existe");

            return usuario;
        }

        public List<Repartidores> ConsultarPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El nombre a buscar no puede estar vacío");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Repartidores!
                .Where(u => u.Nombre != null && u.Nombre.Contains(nombre))
                .ToList();
        }

        public Repartidores Guardar(Repartidores entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Ya se guardó este repartidor");

            decimal suma = 0;
            int contador = 0;

            foreach (var pedido in entidad.Pedidos ?? new List<Pedidos>())
            {
                if (pedido._EstadoPedidoId?.Nombre == "Completado" && pedido.Calificaciones != null)
                {
                    suma += pedido.Calificaciones.Puntaje;
                    contador++;
                }
            }

            entidad.CalificacionPromedio = contador > 0 ? (suma / contador) : 0m;

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Repartidores!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Repartidores",
                Descripcion = $"Se registró un nuevo repartidor en el sistema. Nombre: {entidad.Nombre}, Email: {entidad.Email}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Repartidores Modificar(Repartidores entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El repartidor no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Repartidores!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Repartidores",
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

            var usuario = this.iConexion.Repartidores!.Find(id);
            if (usuario == null)
                throw new Exception("Cliente no encontrado para eliminar");

            this.iConexion.Repartidores.Remove(usuario);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Repartidores",
                Descripcion = $"Se eliminó al repartidor del sistema. Nombre: {usuario.Nombre}, Email: {usuario.Email}.",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }

    }

}
