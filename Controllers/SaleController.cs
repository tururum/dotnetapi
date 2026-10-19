using dotnetapi.DTO;
using dotnetapi.Models;
using Microsoft.AspNetCore.Mvc;
using dotnetapi.Services;

namespace dotnetapi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SaleController : ControllerBase
    {
        private readonly SaleService _service;

        public SaleController(SaleService service) => _service = service;

        [HttpPost]
        public async Task<ActionResult<Venta>> crearVenta([FromBody] VentaDTO request)
        {
            try
            {
                var items = request.Products.Select(p => (p.ProductId, p.Cantidad)).ToList();

                var venta = await _service.CrearVentaAsync(items);
                return Ok(venta);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Listar todas las ventas
        [HttpGet]
        public async Task<ActionResult<List<Venta>>> listarVentas()
        {
            return await _service.ListarVentas();
        }

        //Listar venta por Codigo
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Venta>> listarVentaPorCodigo([FromRoute] string codigo)
        {
            var venta = await _service.ListarVentaPorCodigo(codigo);

            if (venta == null)
                return NotFound();

            return Ok(venta);
        }

        [HttpPost("{codigo}/anular")]
        public async Task<IActionResult> AnularVenta([FromRoute]string codigo)
        {
            var resultado = await _service.AnularVenta(codigo);

            if (resultado == "Venta no encontrada")
                return NotFound();
            if (resultado == "Ya está anulada")
                return Conflict(new { message = resultado });

            return Ok(new { message = resultado });
        }

        [HttpGet("vista/{codigo}")]
        public async Task<ActionResult<List<DetalleVentaVistaModel>>> DetalleVenta([FromRoute] string codigo)
        {
            var resultado = await _service.DettallesVenta(codigo);

            return Ok(resultado);
        }
    }
}
