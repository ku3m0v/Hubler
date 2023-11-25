namespace Hubler.Models;

public class CashRegisterModel
{
    public int Id { get; set; }
    public int SupermarketId { get; set; }
    public int RegisterNumber { get; set; }
    public string StatusName { get; set; }
    public string? EmployeeEmail { get; set; }
}