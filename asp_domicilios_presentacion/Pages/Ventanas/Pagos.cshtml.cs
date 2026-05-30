using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class PagosModel : PageModel
    {
        private IPagosPresentacion _pagosPresentacion;
        private IPedidosPresentacion _pedidosPresentacion;
        private IMetodoPagoPresentacion _metodoPagoPresentacion;
        private IEstadoPagoPresentacion _estadoPagoPresentacion;

        [BindProperty] public List<Pagos>? Lista { get; set; }
        [BindProperty] public Pagos? PagoNuevo { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public SelectList? OptionPedidos { get; set; }
        public SelectList? OptionMetodos { get; set; }
        public SelectList? OptionEstados { get; set; }

        public PagosModel()
        {
            _pagosPresentacion = new PagosPresentacion();
            _pedidosPresentacion = new PedidosPresentacion();
            _metodoPagoPresentacion = new MetodoPagoPresentacion();
            _estadoPagoPresentacion = new EstadoPagoPresentacion();
        }

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
            try
            {
                if (_pagosPresentacion == null) return;

                int rol = ObtenerRolUsuario();
                int usuarioLogueadoId = ObtenerIdUsuarioLogueado();

                if (rol == 1)
                {
                    // 1. ADMIN: Ve todos los registros sin restricciones
                    Lista = _pagosPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                }
                else
                {
                    var todosLosPagos = _pagosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pagos>();

                    // ⚡ INSTANCIACIÓN MANUAL: Necesitamos los pedidos para saber a quién le pertenece cada pago
                    var _pedidosPresentacion = new PedidosPresentacion();
                    var todosLosPedidos = _pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pedidos>();

                    if (rol == 2)
                    {
                        // 2. CLIENTE: Filtramos los pedidos del cliente y sacamos sus IDs
                        var misPedidosIds = todosLosPedidos
                                                .Where(p => p.ClienteId == usuarioLogueadoId)
                                                .Select(p => p.Id)
                                                .ToList();

                        // Traemos los pagos que coincidan con esos números de pedido
                        Lista = todosLosPagos
                                    .Where(p => misPedidosIds.Contains(p.PedidoId))
                                    .ToList();
                    }
                    else if (rol == 3)
                    {
                        // 3. REPARTIDOR: Filtramos los pedidos asignados al repartidor y sacamos sus IDs
                        var misEntregasIds = todosLosPedidos
                                                .Where(p => p.RepartidorId == usuarioLogueadoId)
                                                .Select(p => p.Id)
                                                .ToList();

                        // Traemos los pagos que coincidan con esos números de pedido
                        Lista = todosLosPagos
                                    .Where(p => misEntregasIds.Contains(p.PedidoId))
                                    .ToList();
                    }
                    else
                    {
                        Lista = new List<Pagos>();
                    }
                }

                PagoNuevo = null;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar los pagos: " + ex.Message;
            }
        }

        public void OnPostBtNuevo()
        {
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden registrar pagos.";
                OnPostBtRefrescar();
                return;
            }

            CargarCatalogos();
            PagoNuevo = new Pagos { FechaPago = DateTime.Now, Comision = 0.00m };
            Borrando = false;
        }

        public void OnPostBtModificar(int data)
        {
            // 1. Candado de seguridad para modificación
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para modificar registros financieros.";
                OnPostBtRefrescar();
                return;
            }

            // Refrescamos la lista para asegurarnos de tener los datos frescos en memoria
            OnPostBtRefrescar();
            CargarCatalogos();

            // Buscamos el pago específico que se seleccionó para editar
            PagoNuevo = Lista!.FirstOrDefault(x => x.Id == data);

            // Limpiamos la lista para dar paso visual al formulario de edición
            Lista = null;
            Borrando = false;
        }

        public void OnPostBtGuardar()
        {
            // 1. Candado de seguridad estricto
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Acción exclusiva de administradores.";
                OnPostBtRefrescar();
                return;
            }

            if (PagoNuevo == null) return;
            try
            {
                var _detallePedidoPresentacion = new DetallePedidoPresentacion();
                var todosLosDetalles = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<DetallePedido>();

                var itemsDelPedido = todosLosDetalles.Where(d => d.PedidoId == PagoNuevo.PedidoId).ToList();

                if (itemsDelPedido.Count == 0)
                {
                    ViewData["Mensaje"] = "Error: El pedido seleccionado no tiene productos asignados. No se puede calcular el monto.";
                    OnPostBtRefrescar();
                    return;
                }

                // Sumamos el subtotal de cada producto y lo asignamos al Monto
                PagoNuevo.Monto = itemsDelPedido.Sum(item => item.Subtotal);
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error crítico al calcular el monto del pedido: " + ex.Message;
                OnPostBtRefrescar();
                return;
            }

            decimal porcentajeTasa = 0.00m;
            try
            {
                var metodoSeleccionado = _metodoPagoPresentacion.ConsultarPorIdAsync(PagoNuevo.MetodoPagoId).GetAwaiter().GetResult();

                if (metodoSeleccionado != null)
                {
 
                    porcentajeTasa = metodoSeleccionado.Comision;
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Advertencia de comunicación: No se pudo verificar la tasa del método de pago. " + ex.Message;
                porcentajeTasa = 0.00m;
            }

            PagoNuevo.Comision = PagoNuevo.Monto * (porcentajeTasa / 100m);

            PagoNuevo._PedidoId = null;
            PagoNuevo._MetodoPagoId = null;
            PagoNuevo._EstadoPagoId = null;
            PagoNuevo.Facturas = null;

            if (PagoNuevo.Id == 0)
            {
                PagoNuevo = _pagosPresentacion.GuardarAsync(PagoNuevo).GetAwaiter().GetResult();
            }
            else
            {
                PagoNuevo = _pagosPresentacion.ModificarAsync(PagoNuevo).GetAwaiter().GetResult();
            }

            if (PagoNuevo == null || PagoNuevo.Id == 0) return;

            OnPostBtRefrescar();
        }

        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
            Borrando = false;
        }

        private void CargarCatalogos()
        {
            var pedidos = _pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult();
            OptionPedidos = new SelectList(pedidos, "Id", "Id");

            var metodos = _metodoPagoPresentacion.ConsultarAsync().GetAwaiter().GetResult();
            OptionMetodos = new SelectList(metodos, "Id", "Nombre");

            var estados = _estadoPagoPresentacion.ConsultarAsync().GetAwaiter().GetResult();
            OptionEstados = new SelectList(estados, "Id", "Nombre");
        }
    }
}