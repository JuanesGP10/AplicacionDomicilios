using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class PedidosModel : PageModel
    {
        private IClientesPresentacion _clientesPresentacion;
        private IPedidosPresentacion _pedidosPresentacion;
        private IZonasPresentacion _zonasPresentacion;
        private IProductosPresentacion _productosPresentacion;

        [BindProperty] public Pedidos? PedidoNuevo { get; set; }
        [BindProperty] public bool Borrando { get; set; }
        [BindProperty] public int IdProductoElegido { get; set; }
        [BindProperty] public int CantidadElegida { get; set; }

        public List<Pedidos>? ListaPedidos { get; set; } = new List<Pedidos>();
        public List<Zonas>? ListaZonas { get; set; } = new List<Zonas>();
        public List<Productos>? ListaProductos { get; set; } = new List<Productos>();
        public List<EstadoPedido>? ListaEstadosPedido { get; set; } = new List<EstadoPedido>();
        public List<DetallePedido>? ListaTodosLosDetalles { get; set; } = new List<DetallePedido>();

        public PedidosModel()
        {
            _clientesPresentacion = new ClientesPresentacion();
            _pedidosPresentacion = new PedidosPresentacion();
            _zonasPresentacion = new ZonasPresentacion();
            _productosPresentacion = new ProductosPresentacion();
        }

        public int ObtenerRolUsuario()
        {
            return HttpContext.Session.GetInt32("Rol") ?? 0;
        }

        public int ObtenerIdUsuarioLogueado()
        {
            return HttpContext.Session.GetInt32("IdUsuario") ?? 0;
        }

        private void CargarCatalogos()
        {
            var _estadoPedidoPresentacion = new EstadoPedidoPresentacion();
            var _detallePedidoPresentacion = new DetallePedidoPresentacion();
            var todosLosPedidos = _pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pedidos>();
            ListaZonas = _zonasPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Zonas>();
            ListaProductos = _productosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Productos>();
            ListaEstadosPedido = _estadoPedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<EstadoPedido>();
            ListaTodosLosDetalles = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<DetallePedido>();

            // 4. Identificamos al usuario actual usando tus métodos de sesión
            int rolId = ObtenerRolUsuario();
            int usuarioId = ObtenerIdUsuarioLogueado();

            // 5. APLICACIÓN DE POLÍTICAS DE SEGURIDAD POR ROL
            if (rolId == 1)
            {
                // 👑 ADMINISTRADOR: Ve absolutamente todo el historial del negocio
                ListaPedidos = todosLosPedidos;
            }
            else if (rolId == 2)
            {
                // 👤 CLIENTE: Únicamente ve las órdenes que él mismo ha solicitado
                ListaPedidos = todosLosPedidos.Where(p => p.ClienteId == usuarioId).ToList();
            }
            else if (rolId == 3)
            {
                // 🛵 REPARTIDOR: Únicamente ve los pedidos que el sistema le asignó automáticamente
                ListaPedidos = todosLosPedidos.Where(p => p.RepartidorId == usuarioId).ToList();
            }
            else
            {
                // En caso de que un rol no mapeado intente ver la lista, se la dejamos vacía por seguridad
                ListaPedidos = new List<Pedidos>();
            }
        }
    

        public void OnGet()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuario))
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
                CargarCatalogos();
                PedidoNuevo = null;
                Borrando = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al sincronizar datos: " + ex.Message;
            }
        }

        public void OnPostBtNuevo()
        {
            CargarCatalogos();
            PedidoNuevo = new Pedidos { DetallePedido = new List<DetallePedido>() };
            CantidadElegida = 1; // Valor inicial por defecto
            Borrando = false;
        }

        public void OnPostBtAgregarItem()
        {
            CargarCatalogos();

            if (IdProductoElegido > 0 && CantidadElegida > 0)
            {
                if (PedidoNuevo.DetallePedido == null)
                {
                    PedidoNuevo.DetallePedido = new List<DetallePedido>();
                }

                var prod = ListaProductos!.FirstOrDefault(x => x.Id == IdProductoElegido);
                if (prod != null)
                {
                    PedidoNuevo.DetallePedido.Add(new DetallePedido
                    {
                        ProductoId = prod.Id,
                        Cantidad = CantidadElegida,
                        // Guardamos temporalmente el precio unitario en el detalle
                        PrecioUnitario = prod.Precio
                    });
                }
            }
            else
            {
                ViewData["Mensaje"] = "Seleccione un producto y cantidad válidos.";
            }

            IdProductoElegido = 0;
            CantidadElegida = 1;
        }

        public IActionResult OnPostBtEditar(int id)
        {
            // 🔐 CONTROL DE ROLES: Solo el Administrador (Rol 1) puede editar
            int rolId = ObtenerRolUsuario();
            if (rolId != 1)
            {
                ViewData["Mensaje"] = "Error de seguridad: No tienes permisos para modificar pedidos.";
                CargarCatalogos();
                return Page();
            }

            if (id > 0)
            {
                var todosLosPedidos = _pedidosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Pedidos>();
                PedidoNuevo = todosLosPedidos.FirstOrDefault(p => p.Id == id);

                if (PedidoNuevo != null)
                {
                    var _detallePedidoPresentacion = new DetallePedidoPresentacion();
                    var todosLosDetalles = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<DetallePedido>();

                    // Cargamos sus productos actuales en el carrito de edición
                    PedidoNuevo.DetallePedido = todosLosDetalles.Where(d => d.PedidoId == id).ToList();
                    Borrando = false; // Activamos el modo edición
                }
            }

            CargarCatalogos();
            return Page();
        }

        public IActionResult OnPostBtGuardar()
        {
            try
            {
                if (PedidoNuevo == null) return Page();

                // 1. Validar que el usuario haya agregado al menos un producto al listado en memoria
                if (PedidoNuevo.DetallePedido == null || PedidoNuevo.DetallePedido.Count == 0)
                {
                    ViewData["Mensaje"] = "Error: Debe añadir al menos un producto al pedido antes de guardarlo.";
                    CargarCatalogos();
                    return Page();
                }

                if (PedidoNuevo.Id > 0)
                {
                    _pedidosPresentacion.ModificarAsync(PedidoNuevo).GetAwaiter().GetResult();
                    var _detallePedidoPresentacion = new DetallePedidoPresentacion();
                    var todosDetalles = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<DetallePedido>();
                    var detallesViejos = todosDetalles.Where(d => d.PedidoId == PedidoNuevo.Id).ToList();
                    foreach (var dv in detallesViejos)
                    {
                        int id = dv.Id;
                        _detallePedidoPresentacion.BorrarAsync(id).GetAwaiter().GetResult();
                    }

                    // 3. ... Y guardar los nuevos ítems que quedaron en el carrito (Carrito actualizado)
                    if (PedidoNuevo.DetallePedido != null)
                    {
                        foreach (var item in PedidoNuevo.DetallePedido)
                        {
                            item.PedidoId = PedidoNuevo.Id;
                            // Aquí incluyes tu lógica de ajustar stock si lo requieres
                            _detallePedidoPresentacion.GuardarAsync(item).GetAwaiter().GetResult();
                        }
                    }
                    ViewData["Exito"] = $"¡El pedido #{PedidoNuevo.Id} fue modificado con éxito!";
                    PedidoNuevo = null; // Limpiamos el formulario
                }
                else 
                { 
                    PedidoNuevo.ClienteId = ObtenerIdUsuarioLogueado();
                    PedidoNuevo.EstadoPedidoId = 1; // Predeterminado en 1
                    PedidoNuevo.FechaCreacion = DateTime.Now;

                // 3. Asignación automática del Repartidor Disponible
                    var _repartidoresPresentacion = new RepartidoresPresentacion();
                    var todosLosRepartidores = _repartidoresPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Repartidores>();
                    var repartidorDisponible = todosLosRepartidores.FirstOrDefault(r => r.Disponible == true);

                    if (repartidorDisponible != null)
                    {
                        PedidoNuevo.RepartidorId = repartidorDisponible.Id;
                    }
                    else
                    {
                        ViewData["Mensaje"] = "Advertencia: No hay repartidores disponibles en este momento. El pedido se guardará sin repartidor.";
                    }

                // Conservamos una copia temporal de los ítems agregados por el usuario
                    var itemsParaGuardar = PedidoNuevo.DetallePedido.ToList();

                // Nulificamos temporalmente la lista navegable para que el primer Insert no falle por IDs huérfanos
                    PedidoNuevo.DetallePedido = null;
                    PedidoNuevo.Total = 0.00m;

                // 4. Guardar el Pedido Padre en la Base de Datos para generar su ID autoincremental
                    var pedidoRegistrado = _pedidosPresentacion.GuardarAsync(PedidoNuevo).GetAwaiter().GetResult();

                        if (pedidoRegistrado != null && pedidoRegistrado.Id > 0)
                            {
                            var _detallePedidoPresentacion = new DetallePedidoPresentacion();
                            decimal acumuladorTotal = 0.00m;

                            var todosLosProductos = _productosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Productos>();

                            foreach (var item in itemsParaGuardar)
                            {
                               var prod = todosLosProductos.FirstOrDefault(p => p.Id == item.ProductoId);
                                decimal precioReal = prod != null ? prod.Precio : item.PrecioUnitario;

                                item.PedidoId = pedidoRegistrado.Id;
                                item.PrecioUnitario = precioReal;
                                item.Subtotal = item.Cantidad * precioReal;

                                prod.Stock -= item.Cantidad;
                                _productosPresentacion.ModificarAsync(prod).GetAwaiter().GetResult();

                               acumuladorTotal += item.Subtotal;

                                _detallePedidoPresentacion.GuardarAsync(item).GetAwaiter().GetResult();
                            }

                            pedidoRegistrado.Total = acumuladorTotal;
                            _pedidosPresentacion.ModificarAsync(pedidoRegistrado).GetAwaiter().GetResult();

                            ViewData["Mensaje"] = "¡Pedido procesado con éxito, repartidor asignado y total calculado!";
                            return RedirectToPage("./Pedidos");
                        }
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error crítico al guardar pedido: " + ex.Message;
            }

            OnPostBtRefrescar();
            return Page();
        }

        public IActionResult OnPostBtQuitarItem(int index)
        {
            try
            {
                // 1. Validamos que la lista contenga elementos antes de intentar borrar
                if (PedidoNuevo != null && PedidoNuevo.DetallePedido != null && PedidoNuevo.DetallePedido.Count > 0)
                {
                    // Validamos que el índice enviado sea correcto
                    if (index >= 0 && index < PedidoNuevo.DetallePedido.Count)
                    {
                        // 🔥 Removemos el producto de la lista temporal en la RAM
                        PedidoNuevo.DetallePedido.RemoveAt(index);
                        ViewData["Exito"] = "Producto removido del carrito.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al intentar quitar el producto: " + ex.Message;
            }

        
            CargarCatalogos();

            // 3. Retornamos la misma página manteniendo los datos restantes en el formulario
            return Page();
        }
        public void OnPostBtBorrar(int id)
        {
            try
            {
                int rolId = ObtenerRolUsuario();
                if (rolId != 1)
                {
                    ViewData["Mensaje"] = "Error de seguridad: No tienes permisos de Administrador para eliminar pedidos.";
                    OnPostBtRefrescar();
                }

                if (id > 0)
                {
                    var _detallePedidoPresentacion = new DetallePedidoPresentacion();

                    var todosLosDetalles = _detallePedidoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<DetallePedido>();
                    var detallesDelPedido = todosLosDetalles.Where(d => d.PedidoId == id).ToList();

                    foreach (var detalle in detallesDelPedido)
                    {
                        _detallePedidoPresentacion.BorrarAsync(detalle.Id).GetAwaiter().GetResult();
                    }

                    _pedidosPresentacion.BorrarAsync(id).GetAwaiter().GetResult();

                    ViewData["Exito"] = $"¡El pedido #{id} y todos sus componentes fueron eliminados correctamente del sistema!";
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error crítico al intentar borrar el pedido: " + ex.Message;
            }

            OnPostBtRefrescar();
        }

        public  void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
        }
    }
}
