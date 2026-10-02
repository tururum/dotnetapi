
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
          
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct([FromRoute]int id, Product product){
          return await _productService.EditProduct(id, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id){
          return await _productService.DeleteProduct(id);
        }
    }
}
