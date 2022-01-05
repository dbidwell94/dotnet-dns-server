namespace DNSServer;

public class DNSQuery
{
    public string[] Labels { get; private set; }
    public QType QType { get; private set; }
    public QClass QClass { get; private set; }

    public DNSQuery(string[] labels, QType qType, QClass qClass)
    {
        this.Labels = labels;
        this.QClass = qClass;
        this.QType = qType;
    }

    public byte[] Data
    {
        get
        {
            List<byte> data = new List<byte>();
            data.AddRange(DNSUtils.GetQName(Labels));
            data.AddRange(DNSUtils.IntTo16Bit((int)this.QType));
            data.AddRange(DNSUtils.IntTo16Bit((int)this.QClass));
            return data.ToArray();
        }
    }
}