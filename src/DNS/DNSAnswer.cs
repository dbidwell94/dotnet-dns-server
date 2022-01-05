namespace DNSServer.Answer;

public class DNSAnswer
{
    public string Name { get; }
    public int TTLSeconds { get; }
    public DNSAnswerData AnswerData { get; private set; }

    public DNSAnswer(string domainName, DNSAnswerData answerData, int ttlSeconds = 3600)
    {
        this.Name = domainName;
        this.TTLSeconds = ttlSeconds;
        this.AnswerData = answerData;
    }

    public byte[] Data
    {
        get
        {
            List<byte> data = new List<byte>();
            // QName
            data.AddRange(DNSUtils.GetQName(Name));

            // Type
            {
                int typeInt = (int)this.AnswerData.QType;
                data.AddRange(DNSUtils.IntTo16Bit(typeInt));
            }

            // Class
            {
                int classInt = (int)this.AnswerData.QClass;
                data.AddRange(DNSUtils.IntTo16Bit(classInt));
            }

            // TTL (4 bytes)
            {
                data.AddRange(DNSUtils.IntTo32Bit(TTLSeconds));
            }

            // Data and DataLength
            {
                var rData = this.AnswerData.RData;
                data.AddRange(DNSUtils.IntTo16Bit(rData.Length));
                data.AddRange(rData);
            }

            return data.ToArray();
        }
    }
}