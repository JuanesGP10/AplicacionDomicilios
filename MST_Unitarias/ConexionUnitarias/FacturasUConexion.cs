
using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Interfaces;

namespace MST_Unitarias.ConexionUnitarias
{
    [TestClass]
    public sealed class FacturasUConexion
    {
        [TestMethod]
        public void VerificarConexionBaseDatos_Facturas()
        {
            IConexion conexion = new Conexion();
            conexion.string_conexion = "server=localhost;Integrated Security=True;TrustServerCertificate=true;database=db_domicilios;";

            var lista = conexion.Facturas!.ToList();

            if (lista.Count > 0)
                return;

            throw new Exception("Error: La tabla de Facturas en la base de datos está vacía o no se pudo establecer conexión.");
        }
    }
}
