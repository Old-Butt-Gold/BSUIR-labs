using System.Net;
using System.Net.NetworkInformation;

public record Info(string Name, string MAC, string Details, IPAddress IpAddress, IPAddress IPv4Mask, OperationalStatus Status, string Type);