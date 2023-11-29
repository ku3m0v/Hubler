namespace Hubler.Models;

public class CashRegisterModel
{
    public int Id { get; set; }
    public string SupermarketName { get; set; }
    public int RegisterNumber { get; set; }
    public string StatusName { get; set; }
    public int? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
}