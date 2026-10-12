namespace dotnetapi.Models{
  public class Compra{

   public long Id {get; set;}
    public String CodigoCompra {get; set;}
    public DateTime Date {get; set;}
    public decimal Total {get; set;}
    public String Estado {get; set;} = "COMPLETADA";

    public ICollection<DetalleCompra> Detalles {get; set;} = new List<DetalleCompra>();
  }
}
