
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_negocio.Nucleo;

namespace lib_domicilios_negocio.Implementaciones
{
    public class HistoricosNegocio : IHistoricos
    {
        private IConexion? iConexion;

        public List<Historicos> Consultar()
        {
            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            return this.iConexion.Historicos!.ToList();
        }

        public Historicos Guardar(Historicos entidad)
        {
            if (entidad.Id != 0)
                throw new Exception("Ya se guardó");

            this.iConexion = new Conexion();
            this.iConexion.string_conexion = Configuraciones.obtener("string_conexion");

            this.iConexion.Historicos!.Add(entidad);
            this.iConexion.SaveChanges();
            return entidad;
        }
    }
}

