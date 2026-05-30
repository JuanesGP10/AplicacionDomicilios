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
    public class VehiculosNegocio : IVehiculos
    {
        private Conexion? iConexion;

        public List<Vehiculos> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Vehiculos!.ToList();
        }

        public Vehiculos ConsultarPorPlaca(string placa)
        {
            if (string.IsNullOrEmpty(placa))
                throw new Exception("La placa a consultar no puede estar vacía");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var vehiculo = this.iConexion.Vehiculos!
                .FirstOrDefault(v => v.Placa == placa);

            if (vehiculo == null)
                throw new Exception($"El vehículo con placa {placa} no se encuentra registrado");

            return vehiculo;
        }

        public Vehiculos Guardar(Vehiculos entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Ya se guardó este vehículo");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Vehiculos!.Add(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Vehiculos",
                Descripcion = $"Se registró un nuevo vehículo. Placa: {entidad.Placa}, Modelo: {entidad.Modelo}).",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            this.iConexion.SaveChanges();
            return entidad;
        }

        public Vehiculos Modificar(Vehiculos entidad)
        {
            if (entidad.Id == 0)
                throw new Exception("El vehículo no se puede modificar porque no existe en la base de datos");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Vehiculos!.Update(entidad);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Vehiculos",
                Descripcion = $"Se modificaron las propiedades del vehículo con ID {entidad.Id}. Placa: {entidad.Placa}.",
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

            var vehiculo = this.iConexion.Vehiculos!.Find(id);
            if (vehiculo == null)
                throw new Exception("Vehículo no encontrado para eliminar");

            this.iConexion.Vehiculos.Remove(vehiculo);

            Historicos auditoria = new Historicos
            {
                EntidadAfectada = "Vehiculos",
                Descripcion = $"Se dio de baja del sistema al vehículo con ID {id} (Placa: {vehiculo.Placa}).",
                Fecha = DateTime.Now
            };
            this.iConexion.Historicos!.Add(auditoria);

            return this.iConexion.SaveChanges() > 0;
        }
    }
}
