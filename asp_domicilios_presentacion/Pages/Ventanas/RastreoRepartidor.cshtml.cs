using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class RastreoRepartidorModel : PageModel
    {
        private IRastreoRepartidorPresentacion _rastreoRepartidorPresentacion;
        private IPedidosPresentacion _pedidosPresentacion; 

        [BindProperty] public List<RastreoRepartidor>? Lista { get; set; }
        [BindProperty] public RastreoRepartidor? Rastreo { get; set; }

        public RastreoRepartidorModel()
        {
            _rastreoRepartidorPresentacion = new RastreoRepartidorPresentacion();
            _pedidosPresentacion = new PedidosPresentacion(); 
        }

        public int ObtenerRolUsuario() => HttpContext.Session.GetInt32("Rol") ?? 0;
        public int ObtenerIdUsuario() => HttpContext.Session.GetInt32("IdUsuario") ?? 0;

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
                if (_rastreoRepartidorPresentacion == null) return;

                var todosLosRepartidores = _rastreoRepartidorPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<RastreoRepartidor>();

                int rol = ObtenerRolUsuario();
                int usuarioId = ObtenerIdUsuario();

                if (rol == 1)
                {
                    Lista = todosLosRepartidores;
                }
                else if (rol == 3)
                {
                    Lista = todosLosRepartidores.Where(x => x.RepartidorId == usuarioId).ToList();
                }
                else if (rol == 2)
                {
                    var todosLosPedidos = _pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pedidos>();

                    var repartidoresAsignados = todosLosPedidos
                        .Where(p => p.ClienteId == usuarioId && p.RepartidorId > 0 && (

                            p.EstadoPedidoId == 2 || p.EstadoPedidoId == 2

                        ))
                        .Select(p => p.RepartidorId)
                        .Distinct()
                        .ToList();

                    Lista = todosLosRepartidores
                        .Where(x => repartidoresAsignados.Contains(x.RepartidorId) ||
                                    (x._RepartidorId != null && repartidoresAsignados.Contains(x._RepartidorId.Id)))
                        .ToList();
                }
                else
                {
                    Lista = new List<RastreoRepartidor>();
                }

                Rastreo = null;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar el rastreo de repartidores: " + ex.Message;
            }
        }

        public void OnPostBtVerDetalle(int data)
        {
            try
            {
                OnPostBtRefrescar();

                var repartidorEncontrado = Lista!.FirstOrDefault(x => x.Id == data);

                if (repartidorEncontrado != null)
                {
                    Rastreo = repartidorEncontrado;
                    Lista = null;
                }
                else
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para monitorear este repartidor.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al cargar el detalle del repartidor: " + ex.Message;
            }
        }

        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
        }
    }
}