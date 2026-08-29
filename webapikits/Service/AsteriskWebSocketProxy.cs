using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using System.Net;
namespace webapikits.Service;

public sealed class AsteriskWebSocketProxy
{
    private readonly PbxGatewayOptions _options;
    private readonly ILogger<AsteriskWebSocketProxy> _logger;

    public AsteriskWebSocketProxy(
        IOptions<PbxGatewayOptions> options,
        ILogger<AsteriskWebSocketProxy> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("WebSocket request required");
            return;
        }

        var requestedProtocols = context.WebSockets.WebSocketRequestedProtocols;
        var useSipProtocol = requestedProtocols.Any(x =>
            string.Equals(x, "sip", StringComparison.OrdinalIgnoreCase));

        using var browserSocket = useSipProtocol
            ? await context.WebSockets.AcceptWebSocketAsync("sip")
            : await context.WebSockets.AcceptWebSocketAsync();

        using var asteriskSocket = new ClientWebSocket();

        asteriskSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
        asteriskSocket.Options.Proxy = new NoProxy();

        if (useSipProtocol)
        {
            asteriskSocket.Options.AddSubProtocol("sip");
        }

        var targetUri = new Uri(_options.AsteriskWsUrl);

        _logger.LogInformation(
            "Connecting WebSocket proxy to {Url}, subProtocol={SubProtocol}",
            targetUri,
            useSipProtocol ? "sip" : "none"
        );

        await asteriskSocket.ConnectAsync(targetUri, context.RequestAborted);

        var browserToAsterisk = PumpAsync(
            "browser->asterisk",
            browserSocket,
            asteriskSocket,
            context.RequestAborted);

        var asteriskToBrowser = PumpAsync(
            "asterisk->browser",
            asteriskSocket,
            browserSocket,
            context.RequestAborted);

        await Task.WhenAny(browserToAsterisk, asteriskToBrowser);

        await SafeCloseAsync(browserSocket);
        await SafeCloseAsync(asteriskSocket);
    }

    private async Task PumpAsync(
        string name,
        WebSocket source,
        WebSocket target,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[64 * 1024];

        try
        {
            while (
                source.State == WebSocketState.Open &&
                target.State == WebSocketState.Open &&
                !cancellationToken.IsCancellationRequested)
            {
                var result = await source.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogInformation("WebSocket pump closed: {Name}", name);
                    break;
                }

                await target.SendAsync(
                    new ArraySegment<byte>(buffer, 0, result.Count),
                    result.MessageType,
                    result.EndOfMessage,
                    cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "WebSocket pump error: {Name}", name);
        }
    }

    private static async Task SafeCloseAsync(WebSocket socket)
    {
        try
        {
            if (socket.State == WebSocketState.Open ||
                socket.State == WebSocketState.CloseReceived)
            {
                await socket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "closed",
                    CancellationToken.None);
            }
        }
        catch
        {
        }
    }
    private sealed class NoProxy : IWebProxy
    {
        public ICredentials? Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return destination;
        }

        public bool IsBypassed(Uri host)
        {
            return true;
        }
    }
}