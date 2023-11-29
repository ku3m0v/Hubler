namespace Hubler.DAL.Models;

public class SupermarketSalesSummary
{
    public int SupermarketId { get; set; }
    public string Title { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int TotalSales { get; set; }
}