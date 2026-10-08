using dotnetapi.Context;
using dotnetapi.Enums;
using dotnetapi.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapi.Services{
  public class SaleService{
    private readonly AppDbContext _context;

    public SaleService(AppDbContext context) => _context = context;

    public async Task<Venta> CrearVentaAsync(List<(long ProductId, decimal cantidad)>items){
      using var transaction = await _context.Database.BeginTransactionAsync();

      try{
        var venta = new Venta{
          CodigoVenta = $"VTA-{DateTime.Now:yyyyMMddHHmmss}",
          Date = DateTime.Now
        };

        decimal total = 0;

        foreach(var (ProductId, cantidad) in items)
        {

            var producto = await _context.Products
              .Include(p => p.Recetas)
              .ThenInclude(r => r.Insumo)
              .FirstAsync(p => p.Id == ProductId);


            var detalle = new DetalleVenta
            {
              ProductId = ProductId,
              Cantidad = cantidad,
              PrecioUnitario = producto.Price,
              Subtotal = producto.Price * cantidad
            };

            venta.Detalles.Add(detalle);
            total += detalle.Subtotal;

            if (producto.Tipo == ProductType.SIMPLE){
              if(producto.StockActual < cantidad){
                throw new Exception($"Stock insuficiente para {producto.Name}");
              }

              producto.StockActual -= cantidad;
              _context.Movimientos.Add(new MovimientoInventario{
                  ProductoId = producto.Id,
                  Tipo = MovementType.SALIDA_VENTA,
                  Cantidad = -cantidad,
                  Referencia = venta.CodigoVenta
                  });
            }
            else{
              foreach (var receta in producto.Recetas){
                var descontar = receta.Cantidad * cantidad;
                if(receta.Insumo.ActualStock < descontar)
                  throw new Exception($"Insumo insuficiente: {receta.Insumo.Name}");

                receta.Insumo.ActualStock -= descontar;
                _context.Movimientos.Add(new MovimientoInventario{
                    InsumoId = receta.Insumo.Id,
                    Tipo = MovementType.SALIDA_VENTA,
                    Cantidad = -descontar,
                    Referencia = $"{venta.CodigoVenta} - {producto.Name}"
                    });

              }
            }

        }

        venta.Total = total;
        _context.Ventas.Add(venta);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return venta;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }

  }
}


