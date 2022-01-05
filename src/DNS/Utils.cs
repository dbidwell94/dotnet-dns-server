using System.Net;

namespace DNSServer;

public static class DNSUtils
{
    public static byte[] GetQName(string domain)
    {
        string[] domains = domain.Split('.');
        return GetQName(domains);
    }

    public static byte[] GetQName(string[] domains)
    {
        List<byte> data = new List<byte>();
        foreach (var str in domains)
        {
            data.Add(IntTo8Bit(str.Length));
            data.AddRange(System.Text.Encoding.UTF8.GetBytes(str));
        }
        data.Add(0x0);

        return data.ToArray();
    }

    public static byte[] IntTo16Bit(int input)
    {
        ushort input16;
        if (!UInt16.TryParse(input.ToString(), out input16))
        {
            throw new Exception($"Input was {input}");
        }
        if (BitConverter.IsLittleEndian)
        {
            return BitConverter.GetBytes(input16).Reverse().ToArray();
        }
        return BitConverter.GetBytes(input16);
    }

    public static byte IntTo8Bit(int input)
    {
        return (byte)(input & 255);
    }

    public static byte[] IntTo32Bit(int input)
    {
        uint uint32;
        if (!UInt32.TryParse(input.ToString(), out uint32))
        {
            throw new Exception($"Input was {input}");
        }
        if (BitConverter.IsLittleEndian)
        {
            return BitConverter.GetBytes(uint32).Reverse().ToArray();
        }
        return BitConverter.GetBytes(uint32);
    }
}