namespace Hubler.DAL.Models;

public class Perishable
{
    public int ProductId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string StorageType { get; set; }
}