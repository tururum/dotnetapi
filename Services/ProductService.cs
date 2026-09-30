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

        public async Task<List<Product>> getProducts()
        {
            return await _context.Products.ToListAsync<Product>();
        }
    }
}