namespace Hubler.DAL.Models;

public class Employee
{
    public int Id { get; set; }
    public String Email { get; set; }
    public String PassHash { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public DateTime CreatedDate { get; set; }
    public int SupermarketId { get; set; }
    public int RoleId { get; set; }
    public int? ContentId { get; set; }
    public int? AdminId { get; set; }
}