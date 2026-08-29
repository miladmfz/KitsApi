using Microsoft.Extensions.Options;
using System.Net.Sockets;

namespace webapikits.Service;

public sealed class PbxGatewayHealthService
{
    private readonly PbxGatewayOptions _options;

    public PbxGatewayHealthService(IOptions<PbxGatewayOptions> options)
    {
        _options = options.Value;
    }

    public async Task<bool> CheckTcpAsync(string host, int port, int timeoutMs = 2000)
    {
        try
        {
            using var client = new TcpClient();

            var connectTask = client.ConnectAsync(host, port);
            var timeoutTask = Task.Delay(timeoutMs);

            var finished = await Task.WhenAny(connectTask, timeoutTask);

            return finished == connectTask && client.Connected;
        }
        catch
        {
            return false;
        }
    }

    public async Task<object> GetHealthAsync()
    {
        var amiOk = await CheckTcpAsync(_options.AsteriskHost, _options.AmiPort);

        var wsUri = new Uri(_options.AsteriskWsUrl);
        var wsPort = wsUri.Port > 0
            ? wsUri.Port
            : wsUri.Scheme == "wss" ? 443 : 80;

        var wsOk = await CheckTcpAsync(wsUri.Host, wsPort);

        return new
        {
            ErrCode = 0,
            ErrDesc = "PBX Gateway checked",
            asteriskHost = _options.AsteriskHost,
            ami = amiOk,
            amiPort = _options.AmiPort,
            asteriskWs = wsOk,
            asteriskWsUrl = _options.AsteriskWsUrl
        };
    }
}