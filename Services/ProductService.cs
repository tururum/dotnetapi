using dotnetapi.Context;
using dotnetapi.Enums;
using dotnetapi.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetapi.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        
        //CrearProducts 
        public async Task<Product> CrearProductoAsync(Product product)
        {
            if (!await _context.Categories.AnyAsync(c => c.Id == product.CategoryId))
                throw new ArgumentException($"CategoryId {product.CategoryId} no existe.");

            if (await _context.Products.AnyAsync(p => p.Code == product.Code))
                throw new ArgumentException($"Ya existe un producto con Code {product.Code}.");

            var producto = new Product
            {
                Name = product.Name,
                Price = product.Price,
                Code = product.Code,
                CategoryId = product.CategoryId,
                StockActual = product.StockActual,
                Tipo = product.Tipo,
                Activo = product.Activo,
            };

            if (product.Tipo == ProductType.RECETA)
            {
                if (product.Recetas == null || !product.Recetas.Any())
                    throw new ArgumentException("Un producto RECETA debe tener insumos.");

                foreach (var insumo in product.Recetas)
                {
                    if (!await _context.Insumos.AnyAsync(i => i.Id == insumo.InsumoId))
                        throw new ArgumentException($"InsumoId {insumo.InsumoId} no existe.");

                    producto.Recetas.Add(
                        new Receta
                        {
                            InsumoId = insumo.InsumoId, // no insumo.Insumo.Id
                            Cantidad = insumo.Cantidad,
                        }
                    );
                }
            }

            _context.Products.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }



        //Listar Productos Activos
        public async Task<List<Product>> ListarProductos(){
          var ProductList = await _context.Products
            .Where(p => p.Activo == true)
            .ToListAsync();

          return ProductList;

        }


    }
}
