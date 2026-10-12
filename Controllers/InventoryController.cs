using dotnetapi.DTO;
using dotnetapi.Models;
using dotnetapi.Services;
using Microsoft.AspNetCore.Mvc;
namespace dotnetapi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _service;

        public InventoryController(InventoryService service) => _service = service;


        //Inventario
        [HttpGet]
        public async Task<ActionResult<InventarioDTO>> ListarInventario()
        {
            var lista = await _service.ListarInventario();

            return Ok(lista);
        }

        //Inventario bajoStock
        [HttpGet("lowstock")]
        public async Task<ActionResult<List<Insumo>>> ListarBajoStock()
        {
            var bajoStock = await _service.ListarBajoStock();
            if (!bajoStock.Any())
            {
                return BadRequest("No hay Insumos con bajo stock");
            }

            return Ok(bajoStock);
        }

        //Productos simples bajoStock
        [HttpGet("lowstock/products")]
        public async Task<ActionResult<List<Product>>> ListarProdLowStock()
        {
            var bajoStock = await _service.ListarProdLowStock();

            if (!bajoStock.Any())
            {
                return BadRequest("No hay productos simples de bajo stock");
            }
            return Ok(bajoStock);
        }

        //Listar Movimientos
        [HttpGet("movements")]
        public async Task<ActionResult<List<MovimientoInventario>>> ListarMovimientos()
        {
            var movimientos = await _service.ListarMovimientos();

            return movimientos;
        }

        //ListarCompras
        [HttpGet("purchases")]
        public async Task<ActionResult<List<Compra>>> ListarCompras()
        {
            var compras = await _service.ListarCompras();

            return compras;
        }

        //Listar Compra por codigo
        [HttpGet("purchases/{codigo}")]
        public async Task<ActionResult<Compra>> ListarCompraPorCodigo([FromRoute] string codigo)
        {
            var compra = await _service.ListarCompraPorCodigo(codigo);

            return compra;
        }

        //Ajustar Stock
        [HttpPost("ajuste")]
        public async Task<ActionResult<InventarioDTO>> AjusteInventario([FromBody] InventarioDTO items)
        {
            var inventario = await _service.AjusteInventario(items);
            return Ok(inventario);
        }


        //RegistrarCompra
        [HttpPost("compra")]
        public async Task<ActionResult<Compra>> GenerarCompra([FromBody] CompraDTO items)
        {
            var lista = items.Insumos.Select(i => (i.InsumoId, i.Cantidad, i.PrecioUnitario)).ToList();

            var compra = await _service.RegistrarCompra(lista);

            return Ok(compra);
        }

        //RegistarInsumo
        [HttpPost("insumo")]
        public async Task<ActionResult<Insumo>> RegistrarInsumo([FromBody] Insumo item)
        {
            var insumo = await _service.RegistrarInsumo(item);

            return Ok(insumo);
        }

        //EliminarInsumo
        [HttpDelete("insumo/{id}")]
        public async Task<ActionResult<string>> EliminarInsumo([FromRoute] long id)
        {
            var insumo = await _service.EliminarInsumo(id);

            return Ok(insumo);
        }

    }
}
