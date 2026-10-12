using dotnetapi.Context;
using dotnetapi.DTO;
using dotnetapi.Enums;
using dotnetapi.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapi.Services
{
    public class InventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context) => _context = context;

        public async Task<InventarioDTO> ListarInventario()
        {
            var Insumos = await _context.Insumos.ToListAsync();

            var productosSimples = await _context
                .Products.Where(p => p.Tipo == ProductType.SIMPLE)
                .ToListAsync();

            return new InventarioDTO { ListInsumo = Insumos, ListProd = productosSimples };
        }

        public async Task<List<Insumo>> ListarBajoStock()
        {
            return await _context.Insumos
                .Where(i => i.ActualStock <= i.StockMinimo)
                .ToListAsync();
        }

        public async Task<List<Product>> ListarProdLowStock()
        {
            return await _context.Products
                .Where(p => p.StockActual < p.StockMinimo && p.Tipo == ProductType.SIMPLE)
                .ToListAsync();
        }

        public async Task<List<MovimientoInventario>> ListarMovimientos()
        {
            return await _context.Movimientos.ToListAsync();
        }

        public async Task<List<Compra>> ListarCompras()
        {
            return await _context.Compras.ToListAsync();
        }

        public async Task<Compra?> ListarCompraPorCodigo(string codigoCompra)
        {
            return await _context.Compras
                .Include(c => c.Detalles)
                .ThenInclude(d => d.Insumo)
                .FirstOrDefaultAsync(c => c.CodigoCompra == codigoCompra);
        }

        public async Task<InventarioDTO> AjusteInventario(InventarioDTO items)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var resultado = new InventarioDTO
            {
                AjusteInsumo = new List<Insumo>(),
                AjusteProd = new List<Product>(),
            };

            try
            {
                if (items.Insumos is not null)
                {
                    foreach (var item in items.Insumos)
                    {
                        var insumo = await _context.Insumos.FirstOrDefaultAsync(i => i.Id == item.InsumoId);
                        var nuevaCantidad = item.Cantidad ?? 0;
                        if (insumo is null) continue;

                        _context.Movimientos.Add(new MovimientoInventario
                        {
                            InsumoId = insumo.Id,
                            Cantidad = nuevaCantidad - insumo.ActualStock,
                            Tipo = MovementType.AJUSTE,
                            Referencia = "AJUSTE"
                        });

                        insumo.ActualStock = nuevaCantidad;
                        resultado.AjusteInsumo!.Add(insumo);
                    }
                }

                if (items.Productos is not null)
                {
                    foreach (var item in items.Productos)
                    {
                        var producto = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductoId);
                        var nuevaCantidad = item.Cantidad;
                        if (producto is null) continue;
                        if (producto.Tipo == ProductType.RECETA)
                            throw new InvalidOperationException("Para actualizar productos tipo receta debe ingresar Insumos");

                        _context.Movimientos.Add(new MovimientoInventario
                        {
                            ProductoId = producto.Id,
                            Cantidad = nuevaCantidad - producto.StockActual,
                            Tipo = MovementType.AJUSTE,
                            Referencia = "AJUSTE"
                        });

                        producto.StockActual = nuevaCantidad;
                        resultado.AjusteProd!.Add(producto);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return resultado;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Compra> RegistrarCompra(List<(long InsumoId, decimal Cantidad, decimal PrecioUnitario)> items)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var compra = new Compra
                {
                    CodigoCompra = $"COMPRA - {DateTime.Now:yyyyMMddHHmmss}",
                    Date = DateTime.Now,
                };

                decimal total = 0;

                foreach (var (insumoId, cantidad, precioUnitario) in items)
                {
                    var insumo = await _context.Insumos.FirstOrDefaultAsync(i => i.Id == insumoId)
                        ?? throw new KeyNotFoundException($"No existe el insumo {insumoId}.");

                    insumo.Price = precioUnitario;
                    insumo.ActualStock = cantidad;

                    var detalle = new DetalleCompra
                    {
                        InsumoId = insumoId,
                        Cantidad = cantidad,
                        PrecioUnitario = precioUnitario,
                        Subtotal = precioUnitario * cantidad,
                    };

                    compra.Detalles.Add(detalle);
                    total += detalle.Subtotal;

                    _context.Movimientos.Add(new MovimientoInventario
                    {
                        InsumoId = insumo.Id,
                        Cantidad = cantidad,
                        Tipo = MovementType.ENTRADA,
                        Referencia = compra.CodigoCompra
                    });
                }

                compra.Total = total;
                _context.Compras.Add(compra);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return compra;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //Agregar insumo
        public async Task<Insumo> RegistrarInsumo(Insumo insumo)
        {
            var newInsumo = new Insumo
            {
                Name = insumo.Name,

                Codigo = insumo.Codigo,

                UnidadMedida = insumo.UnidadMedida,
                StockMinimo = insumo.StockMinimo
            };
            _context.Insumos.Add(newInsumo);
            await _context.SaveChangesAsync();
            return newInsumo;

        }

        public async Task<string> EliminarInsumo(long id)
        {
            var insumo = await _context.Insumos
            .FirstOrDefaultAsync(i => i.Id == id);

            insumo.Activo = false;

            await _context.SaveChangesAsync();

            return "Insumo desactivado";
        }
    }
}
