using dotnetapi.Enums;

namespace dotnetapi.Models{
  public class MovimientoInventario{
    public long Id {get; set;}
    public long? ProductoId {get; set;}
    public long? InsumoId {get; set;}
    public MovementType Tipo {get; set;}
    public decimal Cantidad {get; set;}
    public String? Referencia {get; set;}
    public DateTime Fecha {get; set;} = DateTime.Now;
  }
}
