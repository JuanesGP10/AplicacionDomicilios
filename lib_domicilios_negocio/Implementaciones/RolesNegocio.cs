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
    public class RolesNegocio : IRoles
    {
        private Conexion? iConexion;

        public List<Roles> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Roles!.ToList();
        }

        public Roles ConsultarPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new Exception("El nombre del rol a consultar no puede estar vacío");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            var rol = this.iConexion.Roles!
                .FirstOrDefault(r => r.Nombre == nombre);

            if (rol == null)
                throw new Exception($"El rol '{nombre}' no se encuentra registrado");

            return rol;
        }
    }
}