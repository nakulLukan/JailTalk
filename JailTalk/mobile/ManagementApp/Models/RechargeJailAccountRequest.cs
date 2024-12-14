namespace Management.Models;

public class RechargeJailAccountRequest
{
    public long RequestId { get; set; }
    public byte[] RechargeSecret { get; set; }
    public bool IsCompleteCommand { get; set; }
}
