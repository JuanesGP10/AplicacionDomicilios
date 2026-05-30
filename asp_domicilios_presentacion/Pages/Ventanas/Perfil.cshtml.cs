using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class PerfilModel : PageModel
    {
        // 1. Capas de Presentación / Servicios
        private readonly UsuariosPresentacion _usuariosPresentacion;
        private readonly ClientesPresentacion _clientesPresentacion;
        private readonly RepartidoresPresentacion _repartidoresPresentacion;
        private readonly MetodoPagoPresentacion _metodoPagoPresentacion;
        private readonly VehiculosPresentacion _vehiculosPresentacion;
        private readonly ZonasPresentacion _zonasPresentacion;

        // 2. Propiedades enlazadas para el Front-End
        [BindProperty]
        public Usuarios UsuarioBase { get; set; } = new Usuarios();

        [BindProperty]
        public Clientes? DatosCliente { get; set; }

        [BindProperty]
        public Repartidores? DatosRepartidor { get; set; }

        // 3. Listas para los menús desplegables (<select>)
        public List<MetodoPago> ListaMetodosPago { get; set; } = new List<MetodoPago>();
        public List<Vehiculos> ListaVehiculos { get; set; } = new List<Vehiculos>();
        public List<Zonas> ListaZonas { get; set; } = new List<Zonas>();

        public int RolIdActual { get; set; }

        public PerfilModel()
        {
            _usuariosPresentacion = new UsuariosPresentacion();
            _clientesPresentacion = new ClientesPresentacion();
            _repartidoresPresentacion = new RepartidoresPresentacion();
            _metodoPagoPresentacion = new MetodoPagoPresentacion();
            _vehiculosPresentacion = new VehiculosPresentacion();
            _zonasPresentacion = new ZonasPresentacion();
        }

        // 📥 CARGAR EL PERFIL DE ACUERDO AL ROL EN SESIÓN
        public void OnGet()
        {
            var variable_session = HttpContext.Session.GetString("Usuario");
            if (String.IsNullOrEmpty(variable_session))
            {
                HttpContext.Response.Redirect("/");
                return;
            }

            CargarPerfilPorRol();
        }

        public IActionResult OnPostBtGuardarPerfil()
        {
            try
            {
                int idLogueado = ObtenerIdUsuarioLogueado();
                RolIdActual = ObtenerRolUsuario();

                if (idLogueado == 0)
                {
                    return RedirectToPage("/Login");
                }

                if (RolIdActual == 2) 
                {
                    var todosClientes = _clientesPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Clientes>();
                    var clienteBD = todosClientes.FirstOrDefault(c => c.Id == idLogueado);

                    if (clienteBD != null && DatosCliente != null)
                    {

                        clienteBD.Nombre = UsuarioBase.Nombre;
                        clienteBD.Email = UsuarioBase.Email;
                        clienteBD.Cedula = UsuarioBase.Cedula;

 
                        clienteBD.Direccion = DatosCliente.Direccion;
                        clienteBD.Telefono = DatosCliente.Telefono;
                        clienteBD.MetodoPagoFav = DatosCliente.MetodoPagoFav;

                        _clientesPresentacion.ModificarAsync(clienteBD).GetAwaiter().GetResult();
                        ViewData["Exito"] = "¡Tus datos de perfil y preferencias de cliente se han actualizado con éxito!";
                    }
                }
                else if (RolIdActual == 3) // 🛵 CONTROL DE ROL: REPARTIDOR
                {
                    var todosRepartidores = _repartidoresPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Repartidores>();
                    var repartidorBD = todosRepartidores.FirstOrDefault(r => r.Id == idLogueado);

                    if (repartidorBD != null && DatosRepartidor != null)
                    {
                        repartidorBD.Nombre = UsuarioBase.Nombre;
                        repartidorBD.Email = UsuarioBase.Email;
                        repartidorBD.Cedula = UsuarioBase.Cedula;

                        repartidorBD.Disponible = DatosRepartidor.Disponible;
                        repartidorBD.ZonaId = DatosRepartidor.ZonaId;
                        repartidorBD.VehiculoId = DatosRepartidor.VehiculoId;

                        _repartidoresPresentacion.ModificarAsync(repartidorBD).GetAwaiter().GetResult();
                        ViewData["Exito"] = "¡Tu disponibilidad y datos logísticos se han actualizado con éxito!";
                    }
                }
                else if (RolIdActual == 1) 
                {
                    var todosUsuarios = _usuariosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Usuarios>();
                    var usuarioBD = todosUsuarios.FirstOrDefault(u => u.Id == idLogueado);

                    if (usuarioBD != null)
                    {
                        // Mapeo de atributos directos de la clase base (Usuarios.cs)
                        usuarioBD.Nombre = UsuarioBase.Nombre;
                        usuarioBD.Email = UsuarioBase.Email;
                        usuarioBD.Cedula = UsuarioBase.Cedula;

                        _usuariosPresentacion.ModificarAsync(usuarioBD).GetAwaiter().GetResult();
                        ViewData["Exito"] = "¡Perfil de Administrador actualizado correctamente!";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error crítico al intentar procesar los cambios: " + ex.Message;
            }

            // Recargar catálogos y datos limpios tras el submit
            CargarPerfilPorRol();
            return Page();
        }

        // 🧠 LÓGICA INTERNA DE CARGA INDEPENDIENTE
        private void CargarPerfilPorRol()
        {
            int idLogueado = ObtenerIdUsuarioLogueado();
            RolIdActual = ObtenerRolUsuario();

            // Cargar siempre el catálogo de Métodos de Pago globales
            ListaMetodosPago = _metodoPagoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<MetodoPago>();

            if (RolIdActual == 2) // Cliente
            {
                var todosClientes = _clientesPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Clientes>();
                DatosCliente = todosClientes.FirstOrDefault(c => c.Id == idLogueado);
                UsuarioBase = DatosCliente ?? new Clientes();
            }
            else if (RolIdActual == 3) // Repartidor
            {
                // Carga específica de catálogos operativos únicamente si es Repartidor
                ListaVehiculos = _vehiculosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Vehiculos>();
                ListaZonas = _zonasPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Zonas>();

                var todosRepartidores = _repartidoresPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Repartidores>();
                DatosRepartidor = todosRepartidores.FirstOrDefault(r => r.Id == idLogueado);
                UsuarioBase = DatosRepartidor ?? new Repartidores();
            }
            else 
            {
                var todosUsuarios = _usuariosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Usuarios>();
                UsuarioBase = todosUsuarios.FirstOrDefault(u => u.Id == idLogueado) ?? new Usuarios();
            }
        }

        public int ObtenerIdUsuarioLogueado()
        {
            int? id = HttpContext.Session.GetInt32("IdUsuario");
            return id ?? 0;
        }

        public int ObtenerRolUsuario()
        {
            int? rol = HttpContext.Session.GetInt32("Rol");
            return rol ?? 0;
        }
    }
}