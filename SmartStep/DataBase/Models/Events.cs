using System;

namespace SmartStep.Models;

public class Events
{
    public int ID { get; set; }
    public DateOnly Date { get; set; }
    public int EventName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public int Count { get; set; }
}