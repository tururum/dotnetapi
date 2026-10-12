using dotnetapi.Models;

namespace dotnetapi.DTO{

  public class EditProductDTO{
      public string? Name {get; set;}
      public decimal? Price {get; set;}
      public string? Code {get; set;}
      public long? CategoryId {get; set;}
      public decimal? StockActual {get; set;}
      public decimal? StockMinimo {get; set;}
      public List<Receta>? Recetas {get; set;} = new List<Receta>();
  }
}
