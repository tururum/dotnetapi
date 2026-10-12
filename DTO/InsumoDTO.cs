namespace dotnetapi.DTO
{
    public class InsumoDTO
    {

        public long? InsumoId { get; set; }
        public decimal? Cantidad { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? UnidadMedida { get; set; }
        public decimal? StockMinimo { get; set; }
    }
}
