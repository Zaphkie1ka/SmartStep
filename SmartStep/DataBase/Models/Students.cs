using System;
using System.Xml.Schema;

namespace SmartStep.Models;

public class Students
{
    public int ID { get; set; }
    public int Name { get; set; }
    public DateOnly Birthday { get; set; }
    public string School { get; set; }
    public int Class { get; set; }
    public string Address { get; set; }
    public string Parent_name { get; set; }
    public int Parent_number { get; set; }
    public DateOnly Start_date { get; set; }
}