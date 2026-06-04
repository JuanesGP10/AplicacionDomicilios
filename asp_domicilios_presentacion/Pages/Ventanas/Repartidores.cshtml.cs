using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class RepartidoresModel : PageModel
    {
        private IRepartidoresPresentacion _repartidoresPresentacion;

        [BindProperty] public List<Repartidores>? Lista { get; set; }
        [BindProperty] public Repartidores? Repartidor { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public RepartidoresModel()
        {
            _repartidoresPresentacion = new RepartidoresPresentacion();
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
                if (_repartidoresPresentacion == null) return;
                Lista = _repartidoresPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                Repartidor = null;
                Borrando = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar: " + ex.Message;
            }
        }

        public void OnPostBtNuevo()
        {
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Solo el administrador puede registrar repartidores manualmente.";
                OnPostBtRefrescar();
                return;
            }

            Repartidor = new Repartidores()
            {
                FechaNacimiento = DateTime.Today
            };
            Borrando = false;
        }

        public void OnPostBtModificar(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para modificar repartidores.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Repartidor = Lista!.FirstOrDefault(x => x.Id == data);
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

                if (Repartidor == null) return;

                Repartidor = _repartidoresPresentacion.GuardarAsync(Repartidor).GetAwaiter().GetResult();

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

                if (Repartidor == null) return;

                bool eliminado = _repartidoresPresentacion.BorrarAsync(Repartidor.Id).GetAwaiter().GetResult();
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
                    ViewData["Mensaje"] = "Acceso denegado: No puedes borrar repartidores.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Repartidor = Lista!.FirstOrDefault(x => x.Id == data);
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
