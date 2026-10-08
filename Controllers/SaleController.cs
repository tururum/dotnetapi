using dotnetapi.DTO;
using dotnetapi.Models;

using Microsoft.AspNetCore.Mvc;

namespace dotnetapi.Services{

  [ApiController]
  [Route("api/[Controller]")]
  public class SaleController : ControllerBase{
    
    private readonly SaleService _service;

    public SaleController(SaleService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<Venta>> crearVenta([FromBody] VentaDTO request){

      try
      {
          var items = request.Products
            .Select(p => (p.ProductId, p.Cantidad))
            .ToList();

          var venta= await _service.CrearVentaAsync(items);
          return Created("$/api/sale{venta.Id}", venta);
          
      }
      catch (System.Exception ex)
      {
          
          return BadRequest(new { message = ex.Message})
            ;
      }

    }



  }
}
