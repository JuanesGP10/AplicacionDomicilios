
namespace lib_domicilios_negocio.Nucleo
{
    public class Configuraciones
    {
        public static string obtener(string clave)
        {
            return "server=localhost;database=db_domicilios;Integrated Security=True;TrustServerCertificate=true;";
        }
    }
}
