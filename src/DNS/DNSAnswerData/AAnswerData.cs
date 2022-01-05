using System.Net;

namespace DNSServer.Answer;

public class AAnswerData : DNSAnswerData
{
    public IPAddress Address { get; private set; }
    public AAnswerData(IPAddress address) : base()
    {
        this.QClass = QClass.Internet;
        this.QType = QType.A;
        this.Address = address.MapToIPv4();
    }

    public override byte[] RData
    {
        get
        {

            var data = new byte[4];
            var strAddress = Address.ToString().Split('.');
            if (strAddress == null)
            {
                throw new Exception("Ip Address was invalid");
            }
            for (var i = 0; i < strAddress.Length; i++)
            {
                var parsedInt = int.Parse(strAddress[i]);

                data[i] = DNSUtils.IntTo8Bit(parsedInt);
            }
            return data.ToArray();
        }
    }
}