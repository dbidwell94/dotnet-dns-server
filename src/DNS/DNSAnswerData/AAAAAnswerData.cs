using System.Net;

namespace DotnetDNS.Answer;

public class AAAAAnswerData : DNSAnswerData
{
    public IPAddress IpAddress;
    private byte[] _data = new byte[0];
    public AAAAAnswerData(IPAddress iPAddress)
    {
        this.QType = QType.AAAA;
        this.QClass = QClass.Internet;
        this.IpAddress = iPAddress.MapToIPv6();
    }

    public override byte[] RData
    {
        get
        {
            if (_data.Length < 1)
            {
                byte[] data = IpAddress.GetAddressBytes();
                this._data = data;
            }
            return this._data;
        }
    }
}