namespace dotnetapi.Models{
  public class DetalleVenta{
    public long Id{get; set;}
     public long VentaId {get; set;}
     public Venta Venta {get; set;}
     
     public long ProductId {get; set;}
     public Product Product {get; set;}

     public decimal Cantidad {get; set;}
     public decimal PrecioUnitario {get; set;}
     public decimal Subtotal {get; set;}
  }
}
