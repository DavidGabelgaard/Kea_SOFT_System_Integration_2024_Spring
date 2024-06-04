using StringExtensions;
using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:8080");
var app = builder.Build();
app.UseWebSockets();


var connections = new List<WebSocket>();

app.Map("ws", async context =>
{   //Only accept WebSocket request

    if (context.WebSockets.IsWebSocketRequest)
    {
        
        var wsName = context.Request.Query["name"];

        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        connections.Add(ws);

        await Broadcast($"{wsName} Has joined the room");
        await Broadcast($"{connections.Count} current users");
        await ReceivedMessage(ws, async (result, buffer, socket) =>
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                await Broadcast($"{wsName}: {message}", socket);
            }
            else if (result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
            {
                connections.Remove(ws);
                await Broadcast($"{wsName} has left the room");
                await Broadcast($"There is now {connections.Count} users left in the room");
                await ws.CloseAsync(result.CloseStatus!.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        });

        //    WebSocketState
        //    None = 0,
        //    Connecting = 1,
        //    Open = 2,
        //    CloseSent = 3,
        //    CloseReceived = 4,
        //    Closed = 5,
        //    Aborted = 6

        // while (true)
        // {
        //     var message = ("Hello the time is: " + DateTime.Now.ToString("HH:mm:ss")).ToArraySegment();
        //
        //     if (ws.State == WebSocketState.Open)
        //     {
        //         await ws.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
        //     } else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
        //     {
        //         var closeMessage = ("Connection has closed: " + DateTime.Now.ToString("HH:mm:ss")).ToArraySegment();
        //         await ws.SendAsync(closeMessage, WebSocketMessageType.Text, true, CancellationToken.None);
        //     }
        // }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});







await app.RunAsync();
return;

async Task Broadcast(string message, WebSocket? senderWebSocket = null)
{
    var messageBytes = message.ToArraySegment();
    foreach (var socket in connections)
    {
        if (socket != senderWebSocket && socket.State == WebSocketState.Open)
        {
            await socket.SendAsync(messageBytes, WebSocketMessageType.Text, 0, CancellationToken.None);
        }
    }
}


async Task ReceivedMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[], WebSocket> handleMessage)
{
    var buffer = new byte[1024 * 4];
    while (socket.State == WebSocketState.Open)
    {
        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        handleMessage(result, buffer, socket);
    }
}







