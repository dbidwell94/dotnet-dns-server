
# DotnetDNS

This package allows you to parse a byte array from data sent from a DNS request into comsumable classes and enums. You may then use that request to construct a DNS Response which has a Data property of a byte array type which can be sent back to the endpoint which sent the request.
## Features

- DNS Parsing of raw `byte[]` into consumable classes
- A and AAAA record support
- Cross platform


## Usage/Examples

Written in .net6

```csharp
using DotnetDNS;
using System.Net;

public static async Task Main(string[] args)
{
    var client = new UdpClient(2022);
    while (true)
    {
        var request = await client.ReceiveAsync();
        var dnsRequest = new DNSRequest(request.Buffer);

        var resultCode = RCode.NoError;
        var answers = new List<DNSAnswer>();
        foreach(var query in dnsRequest.Queries)
        {
            DNSAnswerData? answerData = null;
            switch (query.QType)
            {
                case QType.A:
                    answerData = new AAnswerData(System.Net.IPAddress.Parse("192.168.1.1"));
                    break;

                case QType.AAAA:
                    answerData = new AAAAAnswerData(System.Net.IPAddress.Parse("2001:0db8:ac10:fe01::"));
                    break;
            }
            if (answerData == null)
            {
                continue;
            }
            answers.Add(new DNSAnswer(String.Join('.', query.Labels), answerData));
        }
        var dnsResponse = new DNSResponse(dnsRequest, answers.ToArray(), resultCode);
        await client.SendAsync(dnsResponse.Data, dnsResponse.Data.Length, request.RemoteEndPoint);
    }
}
```

