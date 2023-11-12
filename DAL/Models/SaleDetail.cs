namespace Hubler.DAL.Models;

public class SaleDetail
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int QuantitySold { get; set; }
    public decimal TotalPrice { get; set; }
}