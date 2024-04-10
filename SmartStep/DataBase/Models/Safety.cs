using System;

namespace SmartStep.Models;

public class Safety
{
    public int ID { get; set; }
    public int NameStudent { get; set; }
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public bool Check { get; set; }
    public int SafetyName { get; set; }
    public int NameTeacher { get; set; }
}