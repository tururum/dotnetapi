
using dotnetapi.Models;
using Microsoft.AspNetCore.Mvc;
using dotnetapi.Services;

namespace dotnetapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;


        public ProductController(ProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _productService.GetProducts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id){
          return await _productService.GetProductById(id );
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product){
          return await _productService.CreateProduct(product);
        }

    }
}
