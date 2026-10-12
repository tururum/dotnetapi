using dotnetapi.DTO;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> ProductoPorId([FromRoute] long id){
      var producto = await _service.ListarProductoPorId(id);

      return Ok(producto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EditProductDTO>> EditarProducto([FromRoute] long id, EditProductDTO producto){
        var editedproducto = await _service.EditarProducto(producto, id);

        return Ok(editedproducto);

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> EliminarProducto([FromRoute] long id){
      var producto = await _service.EliminarProducto(id);

      return Ok(producto);
    }

  }
}
