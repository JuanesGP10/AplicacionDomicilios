
using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Interfaces;

namespace MST_Unitarias.ConexionUnitarias
{
    [TestClass]
    public sealed class RastreoPedidoUConexion
    {
        [TestMethod]
        public void VerificarConexionBaseDatos_RastreoPedido()
        {
            IConexion conexion = new Conexion();
            conexion.string_conexion = "server=localhost;Integrated Security=True;TrustServerCertificate=true;database=db_domicilios;";

            var lista = conexion.RastreoPedido!.ToList();

            if (lista.Count > 0)
                return;

            throw new Exception("Error: La tabla de RastreoPedido en la base de datos está vacía o no se pudo establecer conexión.");
        }
    }
}
