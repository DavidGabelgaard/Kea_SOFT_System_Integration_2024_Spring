namespace Web_Hooks;

public class WebHookDataObject
{
    public required EventType EventType { get; set; }
    public required CalenderEvent CalenderType { get; set; }
}