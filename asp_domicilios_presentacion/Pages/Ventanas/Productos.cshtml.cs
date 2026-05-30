using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class ProductosModel : PageModel
    {
        private IProductosPresentacion _productosPresentacion;
        private ICategoriasPresentacion _categoriasPresentacion;

        [BindProperty] public List<Productos>? Lista { get; set; }
        [BindProperty] public Productos? Producto { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public SelectList? OptionCategorias { get; set; }

        public ProductosModel()
        {
            _productosPresentacion = new ProductosPresentacion();
            _categoriasPresentacion = new CategoriasPresentacion();
        }

        public int ObtenerRolUsuario()
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
                if (_productosPresentacion == null) return;

                Lista = _productosPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                Producto = null;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al consultar los productos: " + ex.Message;
            }
        }

        public void OnPostBtNuevo()
        {

            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden crear productos.";
                OnPostBtRefrescar();
                return;
            }

            CargarCategorias();
            Producto = new Productos();
            Borrando = false;
        }

        public void OnPostBtModificar(int data)
        {
            try
            {

                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden modificar productos.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                CargarCategorias();
                Producto = Lista!.FirstOrDefault(x => x.Id == data);
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
                    ViewData["Mensaje"] = "Acceso denegado: No tiene permisos para guardar o modificar registros.";
                    OnPostBtRefrescar();
                    return;
                }

                if (Producto == null) return;

                if (Producto.Id == 0)
                    Producto = _productosPresentacion.GuardarAsync(Producto).GetAwaiter().GetResult();
                else
                    Producto = _productosPresentacion.ModificarAsync(Producto).GetAwaiter().GetResult();

                if (Producto == null || Producto.Id == 0) return;

                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                CargarCategorias(); // Recarga el combo para que la vista del formulario no falle
                ViewData["Mensaje"] = "Error al guardar el producto: " + ex.Message;
            }
        }

        public void OnPostBtBorrar()
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden eliminar registros del sistema.";
                    OnPostBtRefrescar();
                    return;
                }

                if (Producto == null) return;

                bool eliminado = _productosPresentacion.BorrarAsync(Producto.Id).GetAwaiter().GetResult();
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error al borrar el producto: " + ex.Message;
            }
        }

        public void OnPostBtBorrarVal(int data)
        {
            try
            {
                if (ObtenerRolUsuario() != 1)
                {
                    ViewData["Mensaje"] = "Acceso denegado: Solo los administradores pueden borrar productos.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Producto = Lista!.FirstOrDefault(x => x.Id == data);
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
            Borrando = false;
        }

        private void CargarCategorias()
        {
            var categorias = _categoriasPresentacion.ConsultarAsync().GetAwaiter().GetResult();
            OptionCategorias = new SelectList(categorias, "Id", "Nombre");
        }
    }
}
