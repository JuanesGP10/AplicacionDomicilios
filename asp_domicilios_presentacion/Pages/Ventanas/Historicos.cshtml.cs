using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class HistoricosModel : PageModel
    {
        private IHistoricosPresentacion _historicosPresentacion;

        [BindProperty] public List<Historicos>? Lista { get; set; }

        public HistoricosModel()
        {
            _historicosPresentacion = new HistoricosPresentacion();
        }

        public void OnGet()
        {
            var variable_session = HttpContext.Session.GetString("Usuario");
            int rol = HttpContext.Session.GetInt32("Rol") ?? 0;

            if (String.IsNullOrEmpty(variable_session) || rol != 1)
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
                if (_historicosPresentacion == null) return;

                Lista = _historicosPresentacion.ConsultarAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar históricos: " + ex.Message;
            }
        }
    }
}
