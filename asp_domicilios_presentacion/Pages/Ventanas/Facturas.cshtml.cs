using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class FacturasModel : PageModel
    {
        private readonly FacturasPresentacion _facturasPresentacion = new FacturasPresentacion();
        private readonly PagosPresentacion _pagosPresentacion = new PagosPresentacion();
        private readonly PedidosPresentacion _pedidosPresentacion = new PedidosPresentacion();
        private readonly DetallePedidoPresentacion _detallePedidoPresentacion = new DetallePedidoPresentacion();
        private readonly ProductosPresentacion _productosPresentacion = new ProductosPresentacion();

        [BindProperty]
        public Facturas FacturaNueva { get; set; } = null!;
        public List<Facturas>? ListaFacturas { get; set; }
        public Dictionary<int, List<string>> ProductosPorFactura { get; set; } = new();
        public List<SelectListItem> OptionPagos { get; set; } = new();
        public bool Borrando { get; set; } = false;

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
            OnPostBtRefrescar();
        }

        public void OnPostBtRefrescar()
        {
            CargarCatalogos();

            var todasLasFacturas = _facturasPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Facturas>();
            var todosLosDetalles = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<DetallePedido>();
            var todosLosProductos = _productosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Productos>();

            int rol = ObtenerRolUsuario();

            if (rol == 1)
            {
                ListaFacturas = todasLasFacturas;
            }
            else
            {
                var pedidosDelCliente = _pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult()
                                            ?.Where(p => p.ClienteId == ObtenerIdUsuarioLogueado())
                                            .Select(p => p.Id).ToList() ?? new List<int>();

                var pagosDePedidosPropios = _pagosPresentacion.ConsultarAsync().GetAwaiter().GetResult()
                                                ?.Where(p => pedidosDelCliente.Contains(p.PedidoId))
                                                .Select(p => p.Id).ToList() ?? new List<int>();

                ListaFacturas = todasLasFacturas.Where(f => pagosDePedidosPropios.Contains(f.PagoId)).ToList();
            }

            ProductosPorFactura.Clear();
            foreach (var fact in ListaFacturas)
            {
                var items = todosLosDetalles.Where(d => d.PedidoId == fact.PedidoId).ToList();
                var listaTextos = new List<string>();

                foreach (var item in items)
                {
                    var prod = todosLosProductos.FirstOrDefault(p => p.Id == item.ProductoId);
                    string nombreProd = prod != null ? prod.Nombre : "Producto Desconocido";
                    listaTextos.Add($"{item.Cantidad}x {nombreProd} (${item.PrecioUnitario:N0} c/u)");
                }
                ProductosPorFactura.Add(fact.Id, listaTextos);
            }

            FacturaNueva = null!;
            Borrando = false;
        }
        public IActionResult OnPostBtGuardar()
        {
            try
            {
                if (FacturaNueva == null) return Page();

                var pagoAsociado = _pagosPresentacion.ConsultarPorIdAsync(FacturaNueva.PagoId).GetAwaiter().GetResult();

                if (pagoAsociado == null)
                {
                    ViewData["Mensaje"] = "El pago seleccionado no es válido o no existe.";
                    CargarCatalogos();
                    return Page();
                }

                decimal subtotal = pagoAsociado.Monto;
                FacturaNueva.PedidoId = pagoAsociado.PedidoId;
                FacturaNueva.Impuesto = subtotal * 0.19m; 
                FacturaNueva.Total = subtotal + FacturaNueva.Impuesto;


                _facturasPresentacion.GuardarAsync(FacturaNueva).GetAwaiter().GetResult();


                ViewData["Mensaje"] = "¡Factura generada y calculada con éxito!";
                return RedirectToPage("./Facturas"); 
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error crítico al generar factura: " + ex.Message;
            }

            OnPostBtRefrescar();
            return Page();
        }
        private void CargarCatalogos()
        {
            var pagos = _pagosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pagos>();
            OptionPagos = pagos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"Pago #{p.Id} (Pedido #{p.PedidoId}) - Monto: ${p.Monto:N2}"
            }).ToList();
        }

        
        public void OnPostBtNuevo() { OnPostBtRefrescar(); CargarCatalogos(); FacturaNueva = new Facturas { FechaEmision = DateTime.Now }; }
        public void OnPostBtCerrar() => OnPostBtRefrescar();
    }
}

