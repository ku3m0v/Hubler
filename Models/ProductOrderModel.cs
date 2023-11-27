namespace Hubler.Models;

public class ProductOrderModel
{
    public int Id { get; set; }
    public string SupermarketName { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
}