using System;

namespace SmartStep.Models;

public class Order
{
    public int ID { get; set; }
    public string Type { get; set; }
    public DateOnly Date { get; set; }
}