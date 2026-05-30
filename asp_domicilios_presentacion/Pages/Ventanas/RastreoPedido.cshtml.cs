
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class RastreoPedidoModel : PageModel
    {
        private IRastreoPedidoPresentacion _rastreoPresentacion;

        [BindProperty] public List<RastreoPedido>? Lista { get; set; }
        [BindProperty] public RastreoPedido? Rastreo { get; set; }

        public RastreoPedidoModel()
        {
            // Instancia de tu capa de presentación de rastreos
            _rastreoPresentacion = new RastreoPedidoPresentacion();
        }

        public int ObtenerRolUsuario()
        {
            return HttpContext.Session.GetInt32("Rol") ?? 0;
        }

        public int ObtenerIdUsuario()
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
            try
            {
                if (_rastreoPresentacion == null) return;

                // 1. Traemos todos los rastreos (aquí x._PedidoId viene nulo, pero x.PedidoId entero sí tiene valor)
                var todosLosRastreos = _rastreoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<RastreoPedido>();

                int rol = ObtenerRolUsuario();
                int usuarioId = ObtenerIdUsuario();

                // 2. CONTROL DE ROLES CONTROLADO POR ENTEROS DIRECTOS
                if (rol == 1)
                {
                    // Admin: Ve todo sin restricciones
                    Lista = todosLosRastreos;
                }
                else if (rol == 2)
                {

                    var pedidosPresentacion = new PedidosPresentacion();
                    var todosLosPedidos = pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pedidos>();

                    var misPedidosIds = todosLosPedidos
                        .Where(p => p.ClienteId == usuarioId && (
   
                            p.EstadoPedidoId == 2 || p.EstadoPedidoId == 3

                        ))
                        .Select(p => p.Id)
                        .ToList();

                    Lista = todosLosRastreos.Where(x => misPedidosIds.Contains(x.PedidoId)).ToList();
                }
                else if (rol == 3)
                {
                    var pedidosPresentacion = new PedidosPresentacion();
                    var todosLosPedidos = pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pedidos>();

                    var misRutasIds = todosLosPedidos
                        .Where(p => p.RepartidorId == usuarioId)
                        .Select(p => p.Id)
                        .ToList();

                    Lista = todosLosRastreos.Where(x => misRutasIds.Contains(x.PedidoId)).ToList();
                }
                else
                {
                    Lista = new List<RastreoPedido>();
                }

                Rastreo = null;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar el rastreo de pedidos: " + ex.Message;
            }
        }

        public void OnPostBtVerDetalle(int data)
        {
            try
            {
                OnPostBtRefrescar();

                var rastreoEncontrado = Lista!.FirstOrDefault(x => x.Id == data);

                if (rastreoEncontrado != null)
                {
                    Rastreo = rastreoEncontrado;
                    Lista = null; 
                }
                else
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para ver este pedido o no existe.";
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al cargar la ubicación: " + ex.Message;
            }
        }

        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
        }
    }
}