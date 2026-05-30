using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class NotificacionesModel : PageModel
    {
        private readonly NotificacionesPresentacion _notificacionesPresentacion = new NotificacionesPresentacion();

        public List<Notificaciones>? ListaNotificaciones { get; set; }

        public int ObtenerRolUsuario()
        {
            return HttpContext.Session.GetInt32("Rol") ?? 0;
        }

        public int ObtenerIdUsuarioLogueado()
        {
            return HttpContext.Session.GetInt32("IdUsuario") ?? 0;
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
   
            var todasLasNotificaciones = _notificacionesPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Notificaciones>();
            int rol = ObtenerRolUsuario();

            if (rol == 1)
            {
                ListaNotificaciones = todasLasNotificaciones.OrderByDescending(n => n.FechaEnvio).ToList();
            }
            else
            {
                ListaNotificaciones = todasLasNotificaciones
                                        .Where(n => n.UsuarioId == ObtenerIdUsuarioLogueado())
                                        .OrderByDescending(n => n.FechaEnvio)
                                        .ToList();
            }
        }

        public IActionResult OnPostBtMarcarLeida(int id)
        {
            var notificacion = _notificacionesPresentacion.ConsultarPorIdAsync(id).GetAwaiter().GetResult();

            if (notificacion != null)
            {
                notificacion.Leida = true;

                notificacion._UsuarioId = null;

                _notificacionesPresentacion.ModificarAsync(notificacion).GetAwaiter().GetResult();
            }

            OnPostBtRefrescar();
            return Page();
        }
        public IActionResult OnPostBtEliminar(int id)
        {
            _notificacionesPresentacion.BorrarAsync(id).GetAwaiter().GetResult();
            OnPostBtRefrescar();
            return Page();
        }
    }
}
