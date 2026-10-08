using dotnetapi.Context;
using dotnetapi.Models;

namespace dotnetapi.Services{
  public class InsumoService{
     public readonly AppDbContext _context;

     public InsumoService(AppDbContext context){
       _context = context;
     }

    public async Task<Insumo> crearInsumo(Insumo insumo){
        var NewInsumo = new Insumo{
          Codigo = insumo.Codigo,
          Name = insumo.Name,
          UnidadMedida = insumo.UnidadMedida,
          Price = insumo.Price,
          StockMinimo = insumo.StockMinimo
        };

        _context.Insumos.Add(NewInsumo);
        await _context.SaveChangesAsync();
        return NewInsumo;
    } 
  }
}
