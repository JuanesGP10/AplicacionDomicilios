using lib_domicilios_negocio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace lib_domicilios_negocio.Interfaces
{
    public interface IConexion
    {
        string? string_conexion { get; set; }

        DbSet<Usuarios>? Usuarios { get; set; }
        DbSet<Roles>? Roles { get; set; }
        DbSet<Repartidores>? Repartidores { get; set; }
        DbSet<Clientes>? Clientes { get; set; }
        DbSet<Productos>? Productos { get; set; }
        DbSet<Categorias>? Categorias { get; set; }
        DbSet<Pedidos>? Pedidos { get; set; }
        DbSet<EstadoPedido>? EstadoPedido { get; set; }
        DbSet<RastreoPedido>? RastreoPedido { get; set; }
        DbSet<RastreoRepartidor>? RastreoRepartidor { get; set; }
        DbSet<Zonas>? Zonas { get; set; }
        DbSet<Vehiculos>? Vehiculos { get; set; }
        DbSet<RutaEntrega>? RutaEntrega { get; set; }
        DbSet<DetallePedido>? DetallePedido { get; set; }
        DbSet<Pagos>? Pagos { get; set; }
        DbSet<MetodoPago>? MetodoPago { get; set; }
        DbSet<EstadoPago>? EstadoPago { get; set; }
        DbSet<Calificaciones>? Calificaciones { get; set; }
        DbSet<Notificaciones>? Notificaciones { get; set; }
        DbSet<Facturas>? Facturas { get; set; }
        DbSet<Historicos>? Historicos { get; set; }

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}
