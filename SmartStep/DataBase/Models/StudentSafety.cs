using System;

namespace SmartStep.Models;

public class StudentSafety
{
    public int ID { get; set; }
    public int StudentName { get; set; }
    public int SafetyName { get; set; }
    public DateOnly Date { get; set; }
}