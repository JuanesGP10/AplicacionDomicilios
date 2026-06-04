
using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Interfaces;

namespace MST_Unitarias.ConexionUnitarias
{
    [TestClass]
    public sealed class EstadoPedidoUConexion
    {
        [TestMethod]
        public void VerificarConexionBaseDatos_EstadoPedido()
        {
            IConexion conexion = new Conexion();
            conexion.string_conexion = "server=localhost;Integrated Security=True;TrustServerCertificate=true;database=db_domicilios;";

            var lista = conexion.EstadoPedido!.ToList();

            if (lista.Count > 0)
                return;

            throw new Exception("Error: La tabla de EstadoPedido en la base de datos está vacía o no se pudo establecer conexión.");
        }
    }
}