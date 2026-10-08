namespace dotnetapi.Models{

  public class Venta{
    public long Id {get; set;}
    public String CodigoVenta {get; set;}
    public DateTime Date {get; set;}
    public decimal Total {get; set;}
    public String Estado {get; set;} = "COMPLETADA";

    public ICollection<DetalleVenta> Detalles {get; set;} = new List<DetalleVenta>();
  }
}
