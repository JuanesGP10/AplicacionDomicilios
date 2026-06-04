using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class FacturasImpresionModel : PageModel
    {
        private readonly FacturasPresentacion _facturasPresentacion = new FacturasPresentacion();
        private readonly DetallePedidoPresentacion _detallePedidoPresentacion = new DetallePedidoPresentacion();
        private readonly ProductosPresentacion _productosPresentacion = new ProductosPresentacion();
        private readonly PedidosPresentacion _pedidosPresentacion = new PedidosPresentacion();
        private readonly UsuariosPresentacion _usuariosPresentacion = new UsuariosPresentacion(); // ⚡ Instanciado para traer los datos del cliente

        public Facturas Factura { get; set; } = null!;
        public List<DetalleImpresionDTO> Detalles { get; set; } = new();
        public decimal SubtotalCalculado { get; set; } = 0;
        public string NombreCliente { get; set; } = "Cliente General";
        public string CedulaCliente { get; set; } = "No Registrada";

        public class DetalleImpresionDTO
        {
            public int Cantidad { get; set; }
            public string NombreProducto { get; set; } = null!;
            public decimal PrecioUnitario { get; set; }
            public decimal Subtotal { get; set; }
        }

        public IActionResult OnGet(int id)
        {
            var fact = _facturasPresentacion.ConsultarPorIdAsync(id).GetAwaiter().GetResult();
            if (fact == null) return RedirectToPage("./Facturas");
            Factura = fact;

            var pedido = _pedidosPresentacion.ConsultarPorIdAsync(Factura.PedidoId).GetAwaiter().GetResult();
            if (pedido != null)
            {
                int idCliente = pedido.ClienteId > 0 ? pedido.ClienteId : pedido.ClienteId;
                var cliente = _usuariosPresentacion.ConsultarPorIdAsync(idCliente).GetAwaiter().GetResult();

                if (cliente != null)
                {
                    NombreCliente = cliente.Nombre;
                    CedulaCliente = cliente.Cedula;
                }
            }

            var todosLosDetalles = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<DetallePedido>();
            var todosLosProductos = _productosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Productos>();

            var itemsDelPedido = todosLosDetalles.Where(d => d.PedidoId == Factura.PedidoId).ToList();

            foreach (var item in itemsDelPedido)
            {
                var prod = todosLosProductos.FirstOrDefault(p => p.Id == item.ProductoId);

                Detalles.Add(new DetalleImpresionDTO
                {
                    Cantidad = item.Cantidad,
                    NombreProducto = prod != null ? prod.Nombre! : "Producto General",
                    PrecioUnitario = item.PrecioUnitario,
                    Subtotal = item.Subtotal
                });

                SubtotalCalculado += item.Subtotal;
            }

            return Page();
        }
    }
}
