using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages
{
    public class IndexModel : PageModel
    {
        private IUsuariosPresentacion _usuariosPresentacion;
        private IClientesPresentacion _clientesPresentacion;
        private IRepartidoresPresentacion _repartidoresPresentacion;

        public bool EstaLogueado = false;
        [BindProperty] public bool Registrando { get; set; } = false;

        [BindProperty] public bool CompletandoDatos { get; set; } = false;

        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? Contraseña { get; set; }
        [BindProperty] public string? NombreCompleto { get; set; }
        [BindProperty] public string? Cedula { get; set; }
        [BindProperty] public DateTime FechaNacimiento { get; set; } = DateTime.Today;
        [BindProperty] public string? TipoRegistro { get; set; }

        // Atributos reales extraídos de tu modelo Clientes
        [BindProperty] public string? Direccion { get; set; }
        [BindProperty] public string? Telefono { get; set; }
        [BindProperty] public int MetodoPagoFav { get; set; } = 1;

        // Atributos reales extraídos de tu modelo Repartidores
        [BindProperty] public int VehiculoId { get; set; } = 1;
        [BindProperty] public int ZonaId { get; set; } = 1;

        public IndexModel()
        {
            _usuariosPresentacion = new UsuariosPresentacion();
            _clientesPresentacion = new ClientesPresentacion();
            _repartidoresPresentacion = new RepartidoresPresentacion();
        }

        public void OnGet()
        {
            var variable_session = HttpContext.Session.GetString("Usuario");
            if (!String.IsNullOrEmpty(variable_session))
            {
                EstaLogueado = true;
                return;
            }
        }

        public void OnPostBtClean()
        {
            try
            {
                Email = string.Empty;
                Contraseña = string.Empty;
                NombreCompleto = string.Empty;
                Cedula = string.Empty;
                FechaNacimiento = DateTime.Today;
                TipoRegistro = string.Empty;

                Direccion = string.Empty;
                Telefono = string.Empty;
                MetodoPagoFav = 1;
                VehiculoId = 1;
                ZonaId = 1;

                CompletandoDatos = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
            }
        }

        public void OnPostBtEnter()
        {
            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contraseña))
                {
                    OnPostBtClean();
                    return;
                }

                if (_usuariosPresentacion == null) return;

                List<Usuarios> listaUsuarios = _usuariosPresentacion.ConsultarAsync().GetAwaiter().GetResult();

                Usuarios? usuarioValido = listaUsuarios.FirstOrDefault(x =>
                    x.Email == this.Email &&
                    x.Contrasena == this.Contraseña
                );

                if (usuarioValido != null && usuarioValido.Id > 0)
                {
                    HttpContext.Session.SetInt32("IdUsuario", usuarioValido.Id);
                    HttpContext.Session.SetString("Usuario", usuarioValido.Email!);
                    HttpContext.Session.SetInt32("Rol", usuarioValido.Rol);

                    if (!string.IsNullOrEmpty(usuarioValido.Nombre))
                    {
                        HttpContext.Session.SetString("NombreUsuario", usuarioValido.Nombre);
                    }

                    EstaLogueado = true;
                    OnPostBtClean();
                }
                else
                {
                    ViewData["Mensaje"] = "El correo electrónico o la contraseña son incorrectos.";
                    EstaLogueado = false;
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al conectar con el servicio de usuarios: " + ex.Message;
            }
        }

        public void OnPostBtClose()
        {
            try
            {
                HttpContext.Session.Clear();
                EstaLogueado = false;
                Registrando = false;
                CompletandoDatos = false;
                HttpContext.Response.Redirect("/");
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
            }
        }

        public void OnPostBtIrARegistro()
        {
            OnPostBtClean();
            Registrando = true;
        }

        public void OnPostBtCancelarRegistro()
        {
            OnPostBtClean();
            Registrando = false;
            CompletandoDatos = false;
        }

        public void OnPostBtConfirmarRegistro()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Contraseña) || string.IsNullOrEmpty(TipoRegistro))
            {
                ViewData["Mensaje"] = "Por favor, complete todos los campos obligatorios.";
                Registrando = true;
                CompletandoDatos = false;
                return;
            }

            // Cambiamos de estado para exigir los datos dinámicos en la pantalla
            Registrando = false;
            CompletandoDatos = true;
        }

        public void OnPostBtFinalizarRegistro()
        {
            try
            {
                if (TipoRegistro == "Cliente")
                {
                    Clientes nuevoCliente = new Clientes()
                    {
                        Id = 0,
                        Nombre = this.NombreCompleto,
                        Cedula=this.Cedula,
                        Email = this.Email,
                        Contrasena = this.Contraseña,
                        Rol = 2,
                        Activo = true,
                        FechaNacimiento=this.FechaNacimiento,

                        Direccion = this.Direccion,
                        Telefono = this.Telefono,
                        FechaRegistro = DateTime.Now,
                        MetodoPagoFav = this.MetodoPagoFav
                    };

                    _clientesPresentacion.GuardarAsync(nuevoCliente).GetAwaiter().GetResult();

                    HttpContext.Session.SetInt32("IdUsuario", nuevoCliente.Id);
                    HttpContext.Session.SetString("Usuario", nuevoCliente.Email!);
                    HttpContext.Session.SetInt32("Rol", nuevoCliente.Rol);
                    if (!string.IsNullOrEmpty(nuevoCliente.Nombre))
                    {
                        HttpContext.Session.SetString("NombreUsuario", nuevoCliente.Nombre);
                    }

                    ViewData["Exito"] = "¡Cliente registrado con éxito!";
                }
                else if (TipoRegistro == "Repartidor")
                {
                    // Instanciamos el objeto hijo directo del repartidor
                    Repartidores nuevoRepartidor = new Repartidores()
                    {
                        Id = 0,
                        Nombre = this.NombreCompleto,
                        Cedula = this.Cedula,
                        Email = this.Email,
                        Contrasena = this.Contraseña,
                        Rol = 3,
                        Activo = true,
                        FechaNacimiento = this.FechaNacimiento,

                        // --- Datos específicos de Repartidores ---
                        VehiculoId = this.VehiculoId,
                        ZonaId = this.ZonaId,
                        Disponible = true,
                        CalificacionPromedio = 0.0m,
                    };

                    // Un solo guardado para el repartidor completo
                    _repartidoresPresentacion.GuardarAsync(nuevoRepartidor).GetAwaiter().GetResult();

                    // Login automático para el repartidor
                    HttpContext.Session.SetInt32("IdUsuario", nuevoRepartidor.Id);
                    HttpContext.Session.SetString("Usuario", nuevoRepartidor.Email!);
                    HttpContext.Session.SetInt32("Rol", nuevoRepartidor.Rol);
                    if (!string.IsNullOrEmpty(nuevoRepartidor.Nombre))
                    {
                        HttpContext.Session.SetString("NombreUsuario", nuevoRepartidor.Nombre);
                    }

                    ViewData["Exito"] = "¡Repartidor registrado con éxito!";
                }

                EstaLogueado = true;
                CompletandoDatos = false;
                OnPostBtClean();
            }
            catch (Exception ex)
            {
                CompletandoDatos = true;
                ViewData["Mensaje"] = "Error crítico durante el alta: " + ex.Message;
            }
        }
    }
}