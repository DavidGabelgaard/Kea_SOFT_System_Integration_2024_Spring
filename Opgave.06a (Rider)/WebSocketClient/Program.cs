using System.Net.WebSockets;
using System.Text;

var ws = new ClientWebSocket();

string? name;
Console.WriteLine("Input name: ");
   name = Console.ReadLine();




Console.WriteLine("Connection to server");
await ws.ConnectAsync(new Uri( $"ws://localhost:8080/ws?name={name}"), CancellationToken.None);
Console.WriteLine("Connected");

var sendMessage = Task.Run(async () =>
{
   while (true)
   {
      var message = Console.ReadLine();
      if (message is null or "exit")
      {
         break;
      }
      var bytes = Encoding.UTF8.GetBytes(message);
      await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
   }
});


var receivedTask = Task.Run(async () =>
{
   var buffer = new byte[1024];
   while (true)
   {
      var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

      if (result.MessageType == WebSocketMessageType.Close)
      {
         Console.WriteLine("Connection closed");
         break;
      }
      
      var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
      Console.WriteLine(message);
   }
});

await Task.WhenAny(sendMessage, receivedTask);

if (ws.State != WebSocketState.Closed)
{
   await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
}
await Task.WhenAll(sendMessage, receivedTask);