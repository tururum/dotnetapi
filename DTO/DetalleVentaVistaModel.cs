using System;

namespace dotnetapi.DTO;

public class DetalleVentaVistaModel
{
    public long ID {get; set;}

    public String CodigoVenta {get;set;}

    public DateTime FechaVenta {get; set;}

    public decimal DetalleCantidad {get; set;}

    public decimal DetallePrecioUnitario {get; set;}
    public decimal DetalleSubTotal {get; set;}
    
    public String Producto {get; set;}

    public String CodigoProducto {get; set;}

    public String CategoriaProducto {get; set;}

    public String InsumoNombre {get; set;}
    public decimal InsumoCantidadReceta {get; set;}

}
