using System;
using System.Runtime.InteropServices.JavaScript;

namespace SmartStep.Models;

public class Students
{
    public int ID { get; set; }
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public DateTime Birthday { get; set; }
    public string School { get; set; }
    public string Class { get; set; }
    public string Address { get; set; }
    public string ParentFirst_Name { get; set; }
    public string ParentLast_Name { get; set; }
    public string ParentNumber { get; set; }
    public int OrderNumber { get; set; }
}