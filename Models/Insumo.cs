namespace dotnetapi.Models{
  public class Insumo{
    public long Id {get; set;}
    public required String Codigo {get; set;}
    public required String Name {get; set;}
    public decimal ActualStock {get; set;}
    public required String UnidadMedida {get; set;}
    public decimal Price {get; set;}
    public decimal StockMinimo {get; set;}
  }
}
