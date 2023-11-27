namespace Hubler.Models;

public class EmployeeModel
{
    public int Id { get; set; }
    public String Email { get; set; }
    public String? Password { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? SupermarketName { get; set; }
    public String RoleName { get; set; }
    public int? AdminId { get; set; }
}