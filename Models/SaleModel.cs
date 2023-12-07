namespace Hubler.Models;

public class SaleModel
{
    public int? SaleId { get; set; }
    public string SupermarketName { get; set; }
    public DateTime SaleDate { get; set; }
    public int? SaleDetailId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int QuantitySold { get; set; }
    public decimal? TotalPrice { get; set; }
}