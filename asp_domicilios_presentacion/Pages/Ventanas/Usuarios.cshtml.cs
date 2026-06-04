using ClosedXML.Excel;
using lib_domicilios_negocio.Modelos;
using lib_domicilios_presentacion.Implementaciones;
using lib_domicilios_presentacion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_domicilios_presentacion.Pages.Ventanas
{
    public class UsuariosModel : PageModel
    {
        private IUsuariosPresentacion _usuariosPresentacion;

        [BindProperty] public List<Usuarios>? Lista { get; set; }
        [BindProperty] public Usuarios? Usuario { get; set; }
        [BindProperty] public bool Borrando { get; set; }

        public UsuariosModel()
        {
            _usuariosPresentacion = new UsuariosPresentacion();
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
                if (_usuariosPresentacion == null) return;
                Lista = _usuariosPresentacion.ConsultarAsync().GetAwaiter().GetResult();
                Usuario = null;
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
                ViewData["Mensaje"] = "Acceso denegado: Solo el administrador puede crear usuarios manualmente.";
                OnPostBtRefrescar();
                return;
            }

            Usuario = new Usuarios()
            {
                Rol = 2, 
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
                    ViewData["Mensaje"] = "Acceso denegado: No tienes permisos para modificar usuarios.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Usuario = Lista!.FirstOrDefault(x => x.Id == data);
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

                if (Usuario == null) return;

                Usuario = _usuariosPresentacion.GuardarAsync(Usuario).GetAwaiter().GetResult();

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

                if (Usuario == null) return;

                bool eliminado = _usuariosPresentacion.BorrarAsync(Usuario.Id).GetAwaiter().GetResult();
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
                    ViewData["Mensaje"] = "Acceso denegado: No puedes borrar usuarios.";
                    OnPostBtRefrescar();
                    return;
                }

                OnPostBtRefrescar();
                Usuario = Lista!.FirstOrDefault(x => x.Id == data);
                Lista = null;
                Borrando = true;
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = ex.Message;
            }
        }

        public IActionResult OnGetExportarExcel()
        {
            var listaUsuarios = _usuariosPresentacion.ConsultarAsync().GetAwaiter().GetResult() ?? new List<Usuarios>();

            if (!listaUsuarios.Any())
            {
                ViewData["Mensaje"] = "No existen registros de usuarios para exportar en este momento.";
                return Page();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte de Usuarios");

                var colorHeaderBg = XLColor.FromHtml("#1B365D");
                var colorZebraBg = XLColor.FromHtml("#F2F5F9");

                worksheet.Cell("A1").Value = "REPORTE GENERAL DE USUARIOS DEL SISTEMA";
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Font.FontSize = 16;
                worksheet.Cell("A1").Style.Font.FontColor = colorHeaderBg;
                worksheet.Row(1).Height = 28;

                string[] encabezados = { "ID", "Cédula", "Nombre Completo", "Correo Electrónico", "Fecha Nacimiento", "Rol ID", "Tipo de Cuenta" };
                worksheet.Row(3).Height = 24;

                for (int i = 0; i < encabezados.Length; i++)
                {
                    var celdaHeader = worksheet.Cell(3, i + 1);
                    celdaHeader.Value = encabezados[i];
                    celdaHeader.Style.Font.Bold = true;
                    celdaHeader.Style.Font.FontColor = XLColor.White;
                    celdaHeader.Style.Fill.BackgroundColor = colorHeaderBg;
                    celdaHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    celdaHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    celdaHeader.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                int filaInicio = 4;
                foreach (var user in listaUsuarios)
                {
                    worksheet.Row(filaInicio).Height = 20;

                    worksheet.Cell(filaInicio, 1).Value = user.Id;
                    worksheet.Cell(filaInicio, 2).Value = user.Cedula;
                    worksheet.Cell(filaInicio, 3).Value = user.Nombre;
                    worksheet.Cell(filaInicio, 4).Value = user.Email;
                    worksheet.Cell(filaInicio, 5).Value = user.FechaNacimiento.ToString("dd/MM/yyyy");
                    worksheet.Cell(filaInicio, 6).Value = user.Rol;

                    worksheet.Cell(filaInicio, 7).Value = user.Rol == 1 ? "Administrador" : user.Rol == 2 ? "Cliente" : "Repartidor";

                    worksheet.Cell(filaInicio, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(filaInicio, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(filaInicio, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(filaInicio, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    for (int c = 1; c <= encabezados.Length; c++)
                    {
                        worksheet.Cell(filaInicio, c).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        if (filaInicio % 2 == 0)
                        {
                            worksheet.Cell(filaInicio, c).Style.Fill.BackgroundColor = colorZebraBg;
                        }
                    }

                    filaInicio++;
                }

                worksheet.Row(filaInicio).Height = 22;
                worksheet.Cell(filaInicio, 2).Value = "Total Registrados:";
                worksheet.Cell(filaInicio, 2).Style.Font.Bold = true;
                worksheet.Cell(filaInicio, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                var celdaTotal = worksheet.Cell(filaInicio, 3);
                celdaTotal.FormulaA1 = $"=COUNTA(A4:A{filaInicio - 1})";
                celdaTotal.Style.Font.Bold = true;
                celdaTotal.Style.Border.BottomBorder = XLBorderStyleValues.Double;


                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string nombreArchivo = $"Reporte_Usuarios_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nombreArchivo
                    );
                }
            }
        }
        public void OnPostBtCerrar()
        {
            OnPostBtRefrescar();
            Borrando = false;
        }
    }
}
