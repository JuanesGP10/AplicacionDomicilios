using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class DetallePedidoModel : PageModel
    {
        private IDetallePedidoPresentacion _detallePedidoPresentacion;

        [BindProperty] public List<DetallePedido>? Lista { get; set; }
        [BindProperty] public DetallePedido? Detalle { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public DetallePedidoModel()
        {
            _detallePedidoPresentacion = new DetallePedidoPresentacion();
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

            OnPostBtRefrescar();
        }

        public void OnPostBtRefrescar()
        {
            try
            {
                if (_detallePedidoPresentacion == null) return;
                Lista = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                Detalle = null;
                Borrando = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar: " + ex.Message;
            }
        }

        public void OnPostBtNuevo()
        {
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Solo el administrador puede agregar detalles de pedido.";
                OnPostBtRefrescar();
                return;
            }

            Detalle = new DetallePedido();
            Borrando = false;
        }

        public void OnPostBtModificar(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para modificar el detalle.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Detalle = Lista!.FirstOrDefault(x => x.Id == data);
                Lista = null;
                Borrando = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
            }
        }

        public void OnPostBtGuardar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Sin permisos de escritura.";
                    OnPostBtRefrescar();
                    return;
                }

                if (Detalle == null) return;

                Detalle = _detallePedidoPresentacion.GuardarAsync(Detalle).GetAwaiter().GetResult();

                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al guardar: " + ex.Message;
            }
        }

        public void OnPostBtBorrar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Sin permisos para eliminar.";
                    OnPostBtRefrescar();
                    return;
                }

                if (Detalle == null) return;

                bool eliminado = _detallePedidoPresentacion.BorrarAsync(Detalle.Id).GetAwaiter().GetResult();
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al eliminar: " + ex.Message;
            }
        }

        public void OnPostBtBorrarVal(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No puedes borrar detalles.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Detalle = Lista!.FirstOrDefault(x => x.Id == data);
                Lista = null;
                Borrando = true;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
            }
        }

        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
        }
    }
}
