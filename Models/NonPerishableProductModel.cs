namespace Hubler.Models;

public class NonPerishableProductModel
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public decimal CurrentPrice { get; set; }
    public string ProductType { get; set; }
    public int ShelfLife { get; set; }
}