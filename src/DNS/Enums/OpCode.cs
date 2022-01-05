namespace DotnetDNS;

public enum OpCode
{
    Query = 0,
    /**
    * <summary>IQuery (Obsolete)</summary>
    */
    IQuery = 1,
    Status = 2,
    Unassigned = 3,
    Notify = 4,
    Update = 5,

    /**
    * <summary>DNS Stateful Operations (DSO)</summary>
    */
    DSO = 6
}