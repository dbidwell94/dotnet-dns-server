namespace DNSServer;

public abstract class DNSEntry
{
    public int TransactionId { get; protected set; }
    public bool IsRequest { get; protected set; }
    public OpCode OpCode { get; protected set; }
    public bool IsAuthoritative { get; protected set; }
    public bool RecursionDesired { get; protected set; }
    public bool IsTruncated { get; protected set; }
    public RCode RCode { get; protected set; }
    public bool RecursionAvailable { get; protected set; }

    public int QueryCount { get; protected set; }
    public int AnswerCount { get; protected set; }
    public int NameserverCount { get; protected set; }
    public int AdditionalRecordsCount { get; protected set; }

    protected DNSEntry(in byte[] data)
    {
        this.ParseHeader(data);
    }

    protected DNSEntry(int transactionId, bool isRequest, OpCode opCode, bool isAuthoritative, bool recursionDesired, bool isTruncated, RCode rCode, bool recursionAvailable)
    {
        this.TransactionId = transactionId;
        this.IsRequest = isRequest;
        this.OpCode = opCode;
        this.IsAuthoritative = isAuthoritative;
        this.RecursionDesired = recursionDesired;
        this.IsTruncated = isTruncated;
        this.RCode = rCode;
        this.RecursionAvailable = recursionAvailable;
    }

    protected DNSEntry() { }

    private void ParseHeader(in byte[] data)
    {
        // byte 1, 2
        {
            this.TransactionId = int.Parse(Convert.ToHexString(data.Take(2).ToArray()), NumberStyles.HexNumber);
        }

        // byte 3
        {
            byte b = data[2];
            this.IsRequest = (b & 0b10000000) == 0;
            this.OpCode = (OpCode)(b >> 4);
            this.IsAuthoritative = (b & 0b00000100) != 0;
            this.IsTruncated = (b & 0b00000010) != 0;
            this.RecursionDesired = (b & 0b00000001) != 0;
        }

        // byte 4
        {
            byte b = data[3];
            this.RecursionAvailable = (b & 0b10000000) != 0;
            this.RCode = (RCode)(b & 0b00001111);
        }

        // byte 5, 6
        {
            byte[] b = new byte[] { data[4], data[5] };
            this.QueryCount = int.Parse(Convert.ToHexString(b), NumberStyles.HexNumber);
        }

        // byte 7, 8
        {
            byte[] b = new byte[] { data[6], data[7] };
            this.AnswerCount = int.Parse(Convert.ToHexString(b), NumberStyles.HexNumber);
        }

        // byte 9, 10
        {
            byte[] b = new byte[] { data[8], data[9] };
            this.NameserverCount = int.Parse(Convert.ToHexString(b), NumberStyles.HexNumber);
        }

        // byte 11, 12
        {
            byte[] b = new byte[] { data[10], data[11] };
            this.AdditionalRecordsCount = int.Parse(Convert.ToHexString(b), NumberStyles.HexNumber);
        }
    }

    public abstract byte[] Data { get; }
}