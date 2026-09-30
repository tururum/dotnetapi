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
        
        public async Task<List<Product>> GetProducts(){
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id){
            Product foundProduct = await _context.Products.FindAsync(id);

            if(foundProduct != null){
               return foundProduct;
            }
            throw new Exception("Producto no encontrado");
        }

        public async Task<Product> CreateProduct(Product product){
            _context.Products.Add(product);
              await _context.SaveChangesAsync();
            return product;
            
        }
    }
}
