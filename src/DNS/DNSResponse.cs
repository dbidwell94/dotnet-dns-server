using DNSServer.Answer;

namespace DNSServer;

public class DNSResponse : DNSEntry
{
    public DNSQuery[] Queries { get; private set; }

    public DNSAnswer[] Answers { get; private set; }

    public DNSResponse(DNSRequest originalRequest, DNSAnswer[] answers, RCode resultCode = RCode.NoError)
    {
        this.IsAuthoritative = true;
        this.TransactionId = originalRequest.TransactionId;
        this.IsRequest = false;
        this.OpCode = originalRequest.OpCode;
        this.IsTruncated = false;
        this.RecursionDesired = originalRequest.RecursionDesired;
        this.RecursionAvailable = true;
        this.RCode = resultCode;
        this.QueryCount = originalRequest.QueryCount;
        this.Queries = originalRequest.Queries;
        this.Answers = answers;
        this.AnswerCount = answers.Length;
        this.AdditionalRecordsCount = originalRequest.AdditionalRecordsCount;
    }

    public override byte[] Data
    {
        get
        {
            List<byte> data = new List<byte>();

            // bytes 1, 2
            data.AddRange(DNSUtils.IntTo16Bit(this.TransactionId));


            // byte 3
            {
                byte b = 0x0;
                // QR
                b = (byte)(b | ((!this.IsRequest ? 0x1 : 0x0) << 7));

                // Opcode
                int opCodeInt = (int)this.OpCode;
                byte OpCodeByte = (byte)(opCodeInt << 3);
                b = (byte)(b | OpCodeByte);

                // AuthoritiveAnswer
                b = (byte)(b | ((!this.IsAuthoritative ? 0x1 : 0x0) << 2));

                // Truncated
                b = (byte)(b | ((this.IsTruncated ? 0x1 : 0x0) << 1));

                // RecursionDesired
                b = (byte)((this.RecursionDesired ? 0x1 : 0x0) | b);
                data.Add(b);
            }

            // byte 4
            {
                byte b = 0x0;

                // RecursionAvailable
                b = (byte)(b | ((this.RecursionAvailable ? 0x1 : 0x0) << 7));

                // Result Code
                b = (byte)(b | (((int)this.RCode) & 255));

                data.Add(b);
            }

            // byte 5, 6
            data.AddRange(DNSUtils.IntTo16Bit(this.QueryCount));


            // byte 7, 8
            data.AddRange(DNSUtils.IntTo16Bit(this.AnswerCount));


            // byte 9, 10
            data.AddRange(new byte[] { 0x0, 0x0 });

            // byte 11, 12
            data.AddRange(new byte[] { 0x0, 0x0 });

            foreach (var question in Queries)
            {
                data.AddRange(question.Data);
            }

            foreach (var answer in Answers)
            {
                data.AddRange(answer.Data);
            }

            return data.ToArray();
        }
    }
}