namespace DNSServer;

public class DNSRequest : DNSEntry
{
    public DNSQuery[] Queries { get; private set; }

    public DNSRequest(in byte[] data) : base(data)
    {
        this.Queries = new DNSQuery[this.QueryCount];

        this.ParseQuestions(data.Skip(12).ToArray());
    }



    private void ParseQuestions(byte[] data)
    {
        int questionIndex = 0;
        int endingByteIndex = 0;

        for (int i = 0; i < this.QueryCount; i++)
        {
            List<string> labels = new List<string>();
            QType qType;
            QClass qClass;

            // while condition will break when the checked label length is 0x00
            while (true)
            {
                int labelLength = data[endingByteIndex++];
                if (labelLength == 0x00) break;
                var label = System.Text.Encoding.Default.GetString(data.Skip(endingByteIndex).Take(labelLength).ToArray());
                endingByteIndex += labelLength;
                labels.Add(label);
            }

            // qtype
            {
                byte[] b = new byte[] { data[endingByteIndex++], data[endingByteIndex++] };
                qType = (QType)int.Parse(Convert.ToHexString(b), NumberStyles.HexNumber);
            }

            // qclass
            {
                byte[] b = new byte[] { data[endingByteIndex++], data[endingByteIndex++] };
                qClass = (QClass)int.Parse(Convert.ToHexString(b), NumberStyles.HexNumber);
            }

            this.Queries[questionIndex++] = new DNSQuery(labels.ToArray(), qType, qClass);
        }
    }

    public override byte[] Data
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}
