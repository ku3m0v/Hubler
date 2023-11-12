namespace Hubler.Models;

public class SupermarketWithAddressModel
{
    // Supermarket fields
    public string Title { get; set; }
    public string Phone { get; set; }

    // Address fields
    public string Street { get; set; }
    public int House { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}