namespace Hubler.Models;

public class ProductOrderModel
{
    public int Id { get; set; }
    public string SupermarketName { get; set; }
    public string ProductName { get; set; }
    public DateTime ExpireDate { get; set; }
    public string StorageType { get; set; }
    public int ShelfLife { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
}