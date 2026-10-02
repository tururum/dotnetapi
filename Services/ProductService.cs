using dotnetapi.Models;
using dotnetapi.Context;
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
        
        //Listar todos los Products
        public async Task<List<Product>> GetProducts(){
            return await _context.Products
              .Include(p => p.Category)
              .ToListAsync();
        }

        //Listar producto por Id
        public async Task<Product> GetProductById(int id){
            var foundProduct = await _context.Products 
              .Include(p => p.Category)
              .FirstOrDefaultAsync(p => p.Id == id);

            if(foundProduct != null){
               return foundProduct;
            }
            throw new Exception("Producto no encontrado");
        }
        
        //Crear producto
        public async Task<Product> CreateProduct(Product product){
            _context.Products.Add(product);
              await _context.SaveChangesAsync();
            return product;
            
        }

        
        //Editar producto
        public async Task<Product> EditProduct(int id, Product product){
          var foundProduct = await _context.Products.FindAsync(id);

          if(foundProduct != null){
            foundProduct.Name = product.Name;
            foundProduct.Price = product.Price;
            await _context.SaveChangesAsync();
            return foundProduct;
          }
          throw new Exception("Error");
        }

        //Elminar producto
        public async Task<bool> DeleteProduct(int id){
            var foundProduct = await _context.Products.FindAsync(id);

            if(foundProduct != null){
                _context.Products.Remove(foundProduct);
                await _context.SaveChangesAsync();
                return true;
            }

            throw new Exception("Error");
        }

    }
}
