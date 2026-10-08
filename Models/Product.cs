using dotnetapi.Enums;

namespace dotnetapi.Models
{
    public class Product
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }

        public required String Code {get; set;}

        public long CategoryId { get; set; }
        public Category? Category { get; set; }

        public ProductType Tipo {get; set;} = ProductType.SIMPLE;
        public decimal StockActual {get; set;}
        public bool Activo {get; set;} = true;

        public ICollection<Receta> Recetas {get; set;} = new List<Receta>();

    }
}
