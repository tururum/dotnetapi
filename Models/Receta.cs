namespace dotnetapi.Models{
  public class Receta{
    public long Id {get; set;}
    public long ProductId {get; set;}
    public  Product? Producto {get; set;}

    public long InsumoId {get; set;}
    public Insumo? Insumo {get; set;}

    public decimal Cantidad {get; set;}
  }
}
