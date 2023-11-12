namespace Hubler.DAL.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal CurrentPrice { get; set; }
    public string ProductType { get; set; }
}