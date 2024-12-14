namespace Management.Data;

public class RechargeRequestData
{
    public long RequestId { get; set; }
    public Guid RechargeSecret { get; set; }
    public required string Amount { get; set; }
    public required string PrisonCode { get; set; }
    public required string PrisonName { get; set; }
    public required string RequestedUserFullName { get; set; }
    public required string RequestedUserEmail { get; set; }
}
