namespace Web_Hooks;

public class CalenderEvent
{
    public long? CalenderEventId { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Description { get; set; }
}