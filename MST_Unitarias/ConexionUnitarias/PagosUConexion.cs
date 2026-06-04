
using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Interfaces;

namespace MST_Unitarias.ConexionUnitarias
{
    [TestClass]
    public sealed class PagosUConexion
    {
        [TestMethod]
        public void VerificarConexionBaseDatos_Pagos()
        {
            IConexion conexion = new Conexion();
            conexion.string_conexion = "server=localhost;Integrated Security=True;TrustServerCertificate=true;database=db_domicilios;";

            var lista = conexion.Pagos!.ToList();

            if (lista.Count > 0)
                return;

            throw new Exception("Error: La tabla de Pagos en la base de datos está vacía o no se pudo establecer conexión.");
        }
    }
}
