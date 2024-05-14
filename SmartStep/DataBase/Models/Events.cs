using System;

namespace SmartStep.Models;

public class Events
{
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public int Count { get; set; }
}