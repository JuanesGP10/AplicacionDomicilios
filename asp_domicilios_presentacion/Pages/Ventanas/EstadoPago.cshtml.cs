using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class EstadoPagoModel : PageModel
    {
        private IEstadoPagoPresentacion _estadoPagoPresentacion;

        [BindProperty] public List<EstadoPago>? Lista { get; set; }
        [BindProperty] public EstadoPago? EstadoPagoNuevo { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public EstadoPagoModel()
        {
            _estadoPagoPresentacion = new EstadoPagoPresentacion();
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
                if (_estadoPagoPresentacion == null) return;

                Lista = _estadoPagoPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                EstadoPagoNuevo = null;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar los estados de pago: " + ex.Message;
            }
        }

        public void OnPostBtNuevo()
        {
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden crear estados de pago.";
                OnPostBtRefrescar();
                return;
            }

            EstadoPagoNuevo = new EstadoPago { Activo = true }; 
            Borrando = false;
        }

        public void OnPostBtModificar(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden modificar estados de pago.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                EstadoPagoNuevo = Lista!.FirstOrDefault(x => x.Id == data);
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
                    ViewData["Mensaje"] = "Acceso denegado: No tiene permisos para guardar o modificar registros.";
                    OnPostBtRefrescar();
                    return;
                }

                if (EstadoPagoNuevo == null) return;

                EstadoPagoNuevo.Pagos = null;

                if (EstadoPagoNuevo.Id == 0)
                    EstadoPagoNuevo = _estadoPagoPresentacion.GuardarAsync(EstadoPagoNuevo).GetAwaiter().GetResult();
                else
                    EstadoPagoNuevo = _estadoPagoPresentacion.ModificarAsync(EstadoPagoNuevo).GetAwaiter().GetResult();

                if (EstadoPagoNuevo == null || EstadoPagoNuevo.Id == 0) return;

                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al guardar el estado de pago: " + ex.Message;
            }
        }

        public void OnPostBtBorrar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden eliminar registros.";
                    OnPostBtRefrescar();
                    return;
                }

                if (EstadoPagoNuevo == null) return;

                bool eliminado = _estadoPagoPresentacion.BorrarAsync(EstadoPagoNuevo.Id).GetAwaiter().GetResult();
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al borrar el estado de pago: " + ex.Message;
            }
        }

        public void OnPostBtBorrarVal(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden borrar estados de pago.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                EstadoPagoNuevo = Lista!.FirstOrDefault(x => x.Id == data);
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
            Borrando = false;
        }
    }
}
