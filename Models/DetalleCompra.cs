namespace dotnetapi.Models{
  public class DetalleCompra{
    public long Id{get; set;}
     public long CompraId {get; set;}
     public Compra Compra {get; set;}
     
     public long InsumoId {get; set;}
     public Insumo Insumo {get; set;}

     public decimal Cantidad {get; set;}
     public decimal PrecioUnitario {get; set;}
     public decimal Subtotal {get; set;}
  }
}



































