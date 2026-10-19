using System;

namespace dotnetapi.DTO;

public class DetalleVentaVistaModel
{
    public int ID {get; set;}

    public String CodigoVenta {get;set;}

    public DateTime FechaVenta {get; set;}

    public decimal DetalleCantidad {get; set;}

    public decimal DetallePrecioUnitario {get; set;}
    public decimal DetalleSubTotal {get; set;}
    

}
