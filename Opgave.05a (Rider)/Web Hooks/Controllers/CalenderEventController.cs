using Microsoft.AspNetCore.Mvc;

namespace Web_Hooks.Controllers
{
    [ApiController]
    public class CalenderEventController(IHttpClientFactory httpClientFactory) : ControllerBase
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private WebhookManager _webhookManager = new("WebHooks.txt");
         
        [HttpPost]
        [Route("AddWebHooks")]
        public Task<object> AddWebHooks(ConsumerWebHooks consumerWebHooks)
        {
            if (Uri.TryCreate(consumerWebHooks.Uri.ToString(), UriKind.Absolute, out var uriResult))
            {
                _webhookManager.SaveWebhook(consumerWebHooks);
                return Task.FromResult<object>(Ok("Webhook added successfully."));
            }

            return Task.FromResult<object>(BadRequest("Invalid URI format."));
        }

        [HttpDelete]
        [Route("DeleteWebhook")]
        public void RemoveWebHook(ConsumerWebHooks consumerWebHooks)
        {
            _webhookManager.RemoveWebhook(consumerWebHooks);
        }
        [HttpPost]
        [Route("Ping")]
        public async Task<object> Ping()
        {
            foreach (var webHooks in _webhookManager.GetAllWebhooks())
            {
                await _httpClient.PostAsJsonAsync(webHooks.Uri, "Pinged");
            }
            return Ok("Pinged");
        }

        [HttpPost]
        [Route("Create Event")]
        public async Task<object> CreateEvent(CalenderEvent calenderEvent)
        {
            //Mock Id
            calenderEvent.CalenderEventId = 1213;

            var webHooKDataObject = new WebHookDataObject()
            {
                EventType = EventType.EventCreated,
                CalenderType = calenderEvent,
            };
            
            foreach (var webHooks in _webhookManager.GetAllWebhooks().Where(webHooks => webHooks.WebHookType.Any(x => x == WebHookType.AddEvent)))
            {
                await _httpClient.PostAsJsonAsync(webHooks.Uri,  webHooKDataObject);
            }
            return Ok("Created Event");
        }

        [HttpPut]
        [Route("Move Event")]
        public async Task<object> MoveEvent(long calenderEventId, DateTime startTime, DateTime endTime)
        {
            //Pretend to get a calenderEvent from the db with calenderEventId

            var calenderEvent = new CalenderEvent
            {
                CalenderEventId = calenderEventId,
                StartTime = startTime,
                EndTime = endTime,
                Description = "The same message as the old description"
            };
            
            var webHooKDataObject = new WebHookDataObject()
            {
                EventType = EventType.EventMoved,
                CalenderType = calenderEvent,
            };

            foreach (var webHooks in _webhookManager.GetAllWebhooks().Where(webHooks => webHooks.WebHookType.Any(x => x == WebHookType.MoveEvent)))
            {
                await _httpClient.PostAsJsonAsync(webHooks.Uri, webHooKDataObject);
            }
            return Ok("Event Moved");
        }

        [HttpDelete]
        [Route("Delete Event")]
        public async Task<object> DeleteEvent(long calenderEventId)
        {
            var calenderEvent = new CalenderEvent
            {
                CalenderEventId = calenderEventId,
                StartTime = DateTime.Parse("2025 4 1"),
                EndTime = DateTime.Parse("2025 4 2"),
                Description = "The same message as the old description"
            };
            
            var webHooKDataObject = new WebHookDataObject()
            {
                EventType = EventType.EventDeleted,
                CalenderType = calenderEvent,
            };
            
            foreach (var webHooks in _webhookManager.GetAllWebhooks().Where(webHooks => webHooks.WebHookType.Any(x => x == WebHookType.MoveEvent)))
            {
                await _httpClient.PostAsJsonAsync(webHooks.Uri, webHooKDataObject);
            }
            return Ok("Event Deleted");
        }
    }

}
