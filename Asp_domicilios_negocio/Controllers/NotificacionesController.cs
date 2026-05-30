using lib_domicilios_negocio.Implementaciones;
using lib_domicilios_negocio.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Asp_domicilios_negocio.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NotificacionesController : ControllerBase
    {
        private NotificacionesNegocio? iNotificacionesNegocio;

        public NotificacionesController()
        {
            this.iNotificacionesNegocio = new NotificacionesNegocio();
        }

        [HttpGet]
        public List<Notificaciones> Consultar()
        {
            if (this.iNotificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iNotificacionesNegocio.Consultar();
        }

        [HttpGet]
        public List<Notificaciones> ConsultarPorUsuarioId(int usuarioId)
        {
            if (this.iNotificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iNotificacionesNegocio.ConsultarPorUsuarioId(usuarioId);
        }

        [HttpGet]
        public Notificaciones ConsultarPorId(int id)
        {
            if (this.iNotificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iNotificacionesNegocio.ConsultarPorId(id);
        }

        [HttpPost]
        public Notificaciones Guardar(Notificaciones entidad)
        {
            if (this.iNotificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iNotificacionesNegocio.Guardar(entidad);
        }

        [HttpPut]
        public Notificaciones Modificar(Notificaciones entidad)
        {
            if (this.iNotificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iNotificacionesNegocio.Modificar(entidad);
        }

        [HttpDelete]
        public bool Borrar(int id)
        {
            if (this.iNotificacionesNegocio == null)
                throw new Exception("No implementado");
            return this.iNotificacionesNegocio.Borrar(id);
        }
    }
}
