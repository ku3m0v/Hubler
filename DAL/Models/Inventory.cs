﻿namespace Hubler.DAL.Models;

public class Inventory
{
    public int Id { get; set; }
    public int SupermarketId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}