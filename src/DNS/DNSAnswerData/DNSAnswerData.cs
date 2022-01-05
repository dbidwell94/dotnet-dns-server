namespace DNSServer.Answer;

public abstract class DNSAnswerData
{
    public virtual QType QType { get; protected set; }
    public virtual QClass QClass { get; protected set; }
    public abstract byte[] RData { get; }
}