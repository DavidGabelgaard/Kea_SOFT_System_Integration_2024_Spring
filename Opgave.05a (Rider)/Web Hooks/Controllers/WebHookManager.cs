namespace Web_Hooks.Controllers;

public class WebhookManager(string filePath)
{
    public void SaveWebhook(ConsumerWebHooks webhook)
    {
        var serializedWebhook = $"{webhook.Uri},{string.Join(",", webhook.WebHookType.Select(t => (int)t))}";
        File.AppendAllLines(filePath, new[] { serializedWebhook });
    }

    public void RemoveWebhook(ConsumerWebHooks consumerWebHooks)
    {
        var lines = File.ReadAllLines(filePath).ToList();
        var updatedLines = lines.Where(line => !line.Contains(consumerWebHooks.Uri.ToString()) && !line.Contains(consumerWebHooks.WebHookType.ToString() ?? "null")).ToList();
        File.WriteAllLines(filePath, updatedLines);
    }

    public List<ConsumerWebHooks> GetAllWebhooks()
    {
        var webhooks = new List<ConsumerWebHooks>();
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length >= 2)
            {
                var uri = new Uri(parts[0]);
                var webhookTypes = parts[1].Split(',').Select(int.Parse).Select(x => (WebHookType)x).ToList();
                webhooks.Add(new ConsumerWebHooks { Uri = uri, WebHookType = webhookTypes });
            }
        }
        return webhooks;
    }
}