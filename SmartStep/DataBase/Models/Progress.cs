namespace SmartStep.Models;

public class Progress
{
    public int ID { get; set; }
    public int StudentName { get; set; }
    public int EventName { get; set; }
    public string Result { get; set; }

    // Display-only values filled by a JOIN when loading the table (not columns of `Progress`).
    public string StudentFullName { get; set; } = "";
    public string EventTitle { get; set; } = "";
}
