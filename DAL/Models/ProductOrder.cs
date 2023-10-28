namespace Hubler.DAL.Models;

public class ProductOrder
{
    public int Id { get; set; }
    public int SupermarketId { get; set; }
    public int ProductId { get; set; }
    public int OrderedQuantity { get; set; }
    public DateTime OrderDate { get; set; }
}