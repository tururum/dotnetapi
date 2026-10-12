using dotnetapi.Models;

namespace dotnetapi.DTO{
    public class InventarioDTO{
      public List<InsumoDTO>? Insumos {get; set;}
      public List<ProductoDTO>? Productos {get; set;}

      public List<Product>? AjusteProd {get; set;}
      public List<Insumo>? AjusteInsumo {get; set;}

      public List<Product>? ListProd {get; set;}
      public List<Insumo>? ListInsumo {get; set;}
    }
}
