using dotnetapi.Models;
using dotnetapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapi.Controllers{
  [ApiController]
  [Route("api/[controller]")]
  public class InsumoController : ControllerBase{
    public readonly InsumoService _service;

    public InsumoController(InsumoService service){
      _service = service;
    }
    

    [HttpPost]
    public async Task<ActionResult<Insumo>> PostInsumo(Insumo insumo){
      return await _service.crearInsumo(insumo);
    }
  }
}
