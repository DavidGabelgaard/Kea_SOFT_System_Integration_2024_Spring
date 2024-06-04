namespace Web_Hooks.Controllers;

public class ConsumerWebHooks
{
    public required Uri Uri { get; set; }
    public required List<WebHookType> WebHookType { get; set; }
}