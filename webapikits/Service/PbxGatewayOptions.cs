namespace webapikits.Service;

public sealed class PbxGatewayOptions
{
    public string AsteriskHost { get; set; } = "192.168.1.2";
    public int AmiPort { get; set; } = 5038;
    public string AsteriskWsUrl { get; set; } = "ws://192.168.1.2:8088/ws";
}