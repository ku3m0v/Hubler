namespace Hubler.Models;

public class EmployeeModel
{
    public String Email { get; set; }
    public String Password { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? SupermarketName { get; set; }
    public String Role { get; set; }
    public String AdminEmail { get; set; }
}