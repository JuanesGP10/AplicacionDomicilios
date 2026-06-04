
using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Interfaces;

namespace MST_Unitarias.ConexionUnitarias
{
    [TestClass]
    public sealed class CategoriasUConexion
    {
        [TestMethod]
        public void VerificarConexionBaseDatos_Categorias()
        {
            IConexion conexion = new Conexion();
            conexion.string_conexion = "server=localhost;Integrated Security=True;TrustServerCertificate=true;database=db_domicilios;";

            var lista = conexion.Categorias!.ToList();

            if (lista.Count > 0)
                return;

            throw new Exception("Error: La tabla de Categorias en la base de datos está vacía o no se pudo establecer conexión.");
        }
    }
}
