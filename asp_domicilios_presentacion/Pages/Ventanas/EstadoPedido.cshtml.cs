using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class EstadoPedidoModel : PageModel
    {
        private IEstadoPedidoPresentacion _estadoPedidoPresentacion;

        [BindProperty] public List<EstadoPedido>? Lista { get; set; }
        [BindProperty] public EstadoPedido? Categoria { get; set; } 
        public EstadoPedidoModel()
        {
            _estadoPedidoPresentacion = new EstadoPedidoPresentacion();
        }

        public int ObtenerRolUsuario()
        {
            return HttpContext.Session.GetInt32("Rol") ?? 0;
        }

        public void OnGet()
        {
            var variable_session = HttpContext.Session.GetString("Usuario");
            if (String.IsNullOrEmpty(variable_session))
            {
                HttpContext.Response.Redirect("/");
                return;
            }

            if (ObtenerRolUsuario() != 1)
            {
                HttpContext.Response.Redirect("/Principal");
                return;
            }

            OnPostBtRefrescar();
        }

        public void OnPostBtRefrescar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para ver esta pantalla.";
                    return;
                }

                if (_estadoPedidoPresentacion == null) return;

                Lista = _estadoPedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                Categoria = null;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar estados de pedido: " + ex.Message;
            }
        }

        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
        }
    }
}
