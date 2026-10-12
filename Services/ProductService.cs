using dotnetapi.Context;
using dotnetapi.DTO;
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
                StockMinimo = product.StockMinimo,
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

        //Listar producto por codigo
        public async Task<Product> ListarProductoPorId(long Id){
          var producto = await _context.Products
            .Include(p => p.Recetas)
            .FirstOrDefaultAsync(p => p.Id == Id);

          return producto;
        }

        //Editar Producto

        public async Task<EditProductDTO> EditarProducto(EditProductDTO producto, long id)
        {
            var foundProduct = await _context.Products
              .Include(p => p.Recetas)
              .FirstOrDefaultAsync(p => p.Id == id);

            if (foundProduct is null)
                throw new KeyNotFoundException($"No existe un producto con Id {id}.");

            if (producto.Name is not null) foundProduct.Name = producto.Name;
            if (producto.Price.HasValue) foundProduct.Price = producto.Price.Value;
            if (producto.Code is not null) foundProduct.Code = producto.Code;
            if (producto.StockActual.HasValue && foundProduct.Tipo == ProductType.SIMPLE) foundProduct.StockActual = producto.StockActual.Value;
            if (producto.StockMinimo.HasValue && foundProduct.Tipo == ProductType.SIMPLE) foundProduct.StockMinimo = producto.StockMinimo.Value;
            if (producto.CategoryId.HasValue) foundProduct.CategoryId = producto.CategoryId.Value;

            if (foundProduct.Tipo == ProductType.RECETA)
            {
                var recetasAnteriores = foundProduct.Recetas.ToList();

                _context.Recetas.RemoveRange(recetasAnteriores);
                foundProduct.Recetas.Clear();

                foreach (var receta in producto.Recetas)
                {
                    foundProduct.Recetas.Add(new Receta
                    {
                        InsumoId = receta.InsumoId,
                        Cantidad = receta.Cantidad
                    });
                }
            }

            await _context.SaveChangesAsync();
            return producto;


        }



        //Eliminar Producto
        public async Task<string> EliminarProducto(long Id){
            var foundProduct = await _context.Products
              .FirstOrDefaultAsync(p => p.Id == Id);

            if (foundProduct is not null)
            {
                foundProduct.Activo = false;


                await _context.SaveChangesAsync();

                return "Producto Desactivado";
            }


            throw new Exception("Producto no encontrado");


        }





    }
}
