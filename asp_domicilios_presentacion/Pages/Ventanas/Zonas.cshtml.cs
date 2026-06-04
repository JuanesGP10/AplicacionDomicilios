using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class ZonasModel : PageModel
    {
        private IZonasPresentacion _zonasPresentacion;

        [BindProperty] public List<Zonas>? Lista { get; set; }
        [BindProperty] public Zonas? Zona { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public ZonasModel()
        {
            _zonasPresentacion = new ZonasPresentacion();
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
                if (_zonasPresentacion == null) return;
                Lista = _zonasPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                Zona = null;
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
                ViewData["Mensaje"] = "Acceso denegado: Solo el administrador puede crear zonas.";
                OnPostBtRefrescar();
                return;
            }

            Zona = new Zonas();
            Borrando = false;
        }

        public void OnPostBtEditar(int id)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();

                Zona = Lista!.FirstOrDefault(x => x.Id == id);

                Borrando = false;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al cargar para edición: " + ex.Message;
            }
        }

        public void OnPostBtModificar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para modificar zonas.";
                    OnPostBtRefrescar();
                    return;
                }

                if (Zona != null && Zona.Id > 0)
                {
                    var resultado = _zonasPresentacion.ModificarAsync(Zona).GetAwaiter().GetResult();
                    ViewData["Exito"] = "Zona modificada con éxito.";
                }

                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al modificar: " + ex.Message;
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

                if (Zona == null) return;

                Zona = _zonasPresentacion.GuardarAsync(Zona).GetAwaiter().GetResult();
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

                if (Zona == null) return;

                bool eliminado = _zonasPresentacion.BorrarAsync(Zona.Id).GetAwaiter().GetResult();
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
                    ViewData["Mensaje"] = "Acceso denegado: No puedes borrar zonas.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Zona = Lista!.FirstOrDefault(x => x.Id == data);
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