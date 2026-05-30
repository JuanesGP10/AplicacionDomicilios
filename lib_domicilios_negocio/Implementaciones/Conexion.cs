
using lib_domicilios_negocio.Interfaces;
using lib_domicilios_negocio.Modelos;
using Microsoft.EntityFrameworkCore;

namespace lib_domicilios_negocio.Implementaciones
{
    public class Conexion : DbContext, IConexion
    {
        public string? string_conexion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.string_conexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        public DbSet<Usuarios>? Usuarios { get; set; }
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<Repartidores>? Repartidores { get; set; }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<Productos>? Productos { get; set; }
        public DbSet<Categorias>? Categorias { get; set; }
        public DbSet<Pedidos>? Pedidos { get; set; }
        public DbSet<EstadoPedido>? EstadoPedido { get; set; }
        public DbSet<RastreoPedido>? RastreoPedido { get; set; }
        public DbSet<RastreoRepartidor>? RastreoRepartidor { get; set; }
        public DbSet<Zonas>? Zonas { get; set; }
        public DbSet<Vehiculos>? Vehiculos { get; set; }
        public DbSet<RutaEntrega>? RutaEntrega { get; set; }
        public DbSet<DetallePedido>? DetallePedido { get; set; }
        public DbSet<Pagos>? Pagos { get; set; }
        public DbSet<MetodoPago>? MetodoPago { get; set; }
        public DbSet<EstadoPago>? EstadoPago { get; set; }
        public DbSet<Calificaciones>? Calificaciones { get; set; }
        public DbSet<Notificaciones>? Notificaciones { get; set; }
        public DbSet<Facturas>? Facturas { get; set; }
        public DbSet<Historicos>? Historicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuarios>().ToTable("Usuarios");
            modelBuilder.Entity<Clientes>().ToTable("Clientes");
            modelBuilder.Entity<Repartidores>().ToTable("Repartidores");
        }
    }
}
