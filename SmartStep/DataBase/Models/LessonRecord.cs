using System;

namespace SmartStep.Models;

public class LessonRecord
{
    public Students Name { get; set; }
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public string Hours { get; set; }
    public string Note { get; set; }
}