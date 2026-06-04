using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class VehiculosModel : PageModel
    {
        private IVehiculosPresentacion _vehiculosPresentacion;

        [BindProperty] public List<Vehiculos>? Lista { get; set; }
        [BindProperty] public Vehiculos? Vehiculo { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public VehiculosModel()
        {
            _vehiculosPresentacion = new VehiculosPresentacion();
        }

        private int ObtenerRolUsuario()
        {
            return HttpContext.Session.GetInt32("Rol") ?? 0;
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
                if (_vehiculosPresentacion == null) return;
                Lista = _vehiculosPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                Vehiculo = null;
                Borrando = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al conocer: " + ex.Message;
            }
        }

        public void OnPostBtNuevo()
        {
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Solo el administrador puede registrar vehículos.";
                OnPostBtRefrescar();
                return;
            }

            Vehiculo = new Vehiculos() { Activo = true }; 
            Borrando = false;
        }

        public void OnPostBtModificar(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para modificar vehículos.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Vehiculo = Lista!.FirstOrDefault(x => x.Id == data);
                Lista = null;
                Borrando = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
            }
        }

        public void OnPostBtGuardar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Sin permisos de escritura.";
                    OnPostBtRefrescar();
                    return;
                }

                if (Vehiculo == null) return;

                Vehiculo = _vehiculosPresentacion.GuardarAsync(Vehiculo).GetAwaiter().GetResult();
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al guardar: " + ex.Message;
            }
        }

        public void OnPostBtBorrar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Sin permisos para eliminar.";
                    OnPostBtRefrescar();
                    return;
                }

                if (Vehiculo == null) return;

                bool eliminado = _vehiculosPresentacion.BorrarAsync(Vehiculo.Id).GetAwaiter().GetResult();
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al eliminar: " + ex.Message;
            }
        }

        public void OnPostBtBorrarVal(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No puedes borrar vehículos.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Vehiculo = Lista!.FirstOrDefault(x => x.Id == data);
                Lista = null;
                Borrando = true;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
            }
        }

        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
        }
    }
}
