using Microsoft.AspNetCore.Mvc;

namespace TestingWebHooks.Controllers;

[ApiController]
public class TestingWebHooksController
{
    [HttpPost]
    [Route("WebHook")]
    public void WebHookPosted(object ob)
    {
        Console.WriteLine(ob);
    }
}