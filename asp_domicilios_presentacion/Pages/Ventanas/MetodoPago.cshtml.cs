using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class MetodoPagoModel : PageModel
    {
        private readonly MetodoPagoPresentacion _metodoPagoPresentacion = new MetodoPagoPresentacion();

        [BindProperty]
        public MetodoPago MetodoNuevo { get; set; } = null!;
        public List<MetodoPago>? ListaMetodos { get; set; }
        public bool Borrando { get; set; } = false;

        public int ObtenerRolUsuario()
        {
            return HttpContext.Session.GetInt32("Rol") ?? 0;
        }
        public void OnGet()
        {
            OnPostBtRefrescar();
        }

        public void OnPostBtRefrescar()
        {
            var todos = _metodoPagoPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<MetodoPago>();
            int rol = ObtenerRolUsuario();

            if (rol == 1)
            {
                ListaMetodos = todos;
            }
            else
            {
                ListaMetodos = todos.Where(m => m.Activo).ToList();
            }

            MetodoNuevo = null!;
            Borrando = false;
        }

        public void OnPostBtNuevo()
        {
            OnPostBtRefrescar();
            MetodoNuevo = new MetodoPago
            {
                Comision = 0.00m,
                Activo = true
            };
        }

        public void OnPostBtModificar(int data)
        {
            OnPostBtRefrescar();
            MetodoNuevo = ListaMetodos!.FirstOrDefault(x => x.Id == data)!;
            ListaMetodos = null;
            Borrando = false;
        }

        public void OnPostBtGuardar()
        {
            if (ObtenerRolUsuario() != 1)
            {
                ViewData["Mensaje"] = "Acceso denegado: Solo administradores pueden alterar los parámetros de comisión.";
                OnPostBtRefrescar();
                return;
            }

            if (MetodoNuevo == null) return;

            MetodoNuevo.Clientes = null;
            MetodoNuevo.Pagos = null;

            if (MetodoNuevo.Id == 0)
            {
                MetodoNuevo = _metodoPagoPresentacion.GuardarAsync(MetodoNuevo).GetAwaiter().GetResult();
            }
            else
            {
                MetodoNuevo = _metodoPagoPresentacion.ModificarAsync(MetodoNuevo).GetAwaiter().GetResult();
            }

            OnPostBtRefrescar();
        }

        public void OnPostBtBorrarVal(int data)
        {
            OnPostBtRefrescar();
            MetodoNuevo = ListaMetodos!.FirstOrDefault(x => x.Id == data)!;
            ListaMetodos = null;
            Borrando = true;
        }

        public void OnPostBtBorrar()
        {
            if (ObtenerRolUsuario() != 1) return;

            if (MetodoNuevo != null)
            {
                _metodoPagoPresentacion.BorrarAsync(MetodoNuevo.Id).GetAwaiter().GetResult();
            }
            OnPostBtRefrescar();
        }

        public void OnPostBtCerrar() => OnPostBtRefrescar();
    }
}
