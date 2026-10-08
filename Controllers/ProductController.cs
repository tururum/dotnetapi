using dotnetapi.Models;
using dotnetapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapi.Controllers{
    [ApiController]
    [Route("api/[Controller]")]
  public class ProductController : ControllerBase{
    public readonly ProductService _service;

    public ProductController(ProductService service) => _service = service;


    [HttpPost]
    public async Task<ActionResult<Product>> crearProducto(Product product){
     try
     {
          var created = await _service.CrearProductoAsync(product);
          return Created($"/api/product/{created.Id}", created);
     }
     catch (ArgumentException ex)
     {
         return BadRequest(new { message = ex.Message });
     }
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> ListarProductos(){
        return  await _service.ListarProductos();
    }

  }
}
