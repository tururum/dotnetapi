using dotnetapi.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Venta> Ventas {get; set;}
        public DbSet<DetalleVenta> DetalleVentas {get; set;}
        public DbSet<Insumo> Insumos {get; set;}
        public DbSet<Receta> Recetas {get; set;}
        public DbSet<MovimientoInventario> Movimientos {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
              .HasIndex(p => p.Code).IsUnique();

            modelBuilder.Entity<Insumo>()
              .HasIndex(i => i.Codigo).IsUnique();

            modelBuilder.Entity<Receta>()
              .HasIndex(r => new {r.ProductId, r.InsumoId}).IsUnique();

            modelBuilder.Entity<Venta>()
              .HasIndex(v => v.CodigoVenta).IsUnique();

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var prop in entity.GetProperties())
                {
                    if (prop.ClrType == typeof(decimal) || prop.ClrType == typeof(decimal?))
                        prop.SetColumnType("decimal(18,2)");
                }
            }

              
        }
    }
}
