using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class CategoriasModel : PageModel
    {
        private ICategoriasPresentacion _categoriasPresentacion;

        [BindProperty] public List<Categorias>? ListaCategorias { get; set; } = new List<Categorias>();
        [BindProperty] public Categorias? CategoriaNueva { get; set; } = new Categorias();
        [BindProperty] public bool Borrando { get; set; } = false;

        public CategoriasModel()
        {
            _categoriasPresentacion = new CategoriasPresentacion();
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
                if (_categoriasPresentacion == null) return;
                ListaCategorias = _categoriasPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Categorias>();
                CategoriaNueva = null;
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
                ViewData["Mensaje"] = "Acceso denegado: Solo el administrador puede crear categorías.";
                OnPostBtRefrescar();
                return;
            }

            CategoriaNueva = new Categorias();
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
                CategoriaNueva = ListaCategorias!.FirstOrDefault(x => x.Id == id);
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
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para modificar categorías.";
                    OnPostBtRefrescar();
                    return;
                }

                if (CategoriaNueva != null && CategoriaNueva.Id > 0)
                {
                    if (CategoriaNueva.Creacion < new DateTime(1900, 1, 1))
                    {
                        CategoriaNueva.Creacion = DateTime.Now;
                    }

                    var resultado = _categoriasPresentacion.ModificarAsync(CategoriaNueva).GetAwaiter().GetResult();
                    ViewData["Exito"] = "Categoría modificada con éxito.";
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

                if (CategoriaNueva == null) return;

                CategoriaNueva.Id = 0;
                CategoriaNueva.Creacion = DateTime.Now;
                CategoriaNueva.Activo = true;

                var resultado = _categoriasPresentacion.GuardarAsync(CategoriaNueva).GetAwaiter().GetResult();
                ViewData["Exito"] = "Categoría creada con éxito.";
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al guardar: " + ex.Message;
            }
        }

        public void OnPostBtBorrarVal(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: No puedes borrar categorías.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                CategoriaNueva = ListaCategorias!.FirstOrDefault(x => x.Id == data);
                Borrando = true;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
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

                if (CategoriaNueva == null) return;

                bool eliminado = _categoriasPresentacion.BorrarAsync(CategoriaNueva.Id).GetAwaiter().GetResult();
                ViewData["Exito"] = "Categoría eliminada con éxito.";
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al eliminar: " + ex.Message;
            }
        }

        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
        }
    }
}
