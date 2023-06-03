using JailTalk.Domain.Identity;
using JailTalk.Shared;

namespace JailTalk.Domain.Prison;
public class PhoneBalanceHistory
{
    public int Id { get; set; }
    public float NetAmount { get; set; }
    public float AmountDifference { get; set; }
    public PhoneBalanceReason Reason { get; set; }
    public Guid PrisonerId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public long? CallRequestId { get; set; }
    public string RechargedByUserId { get; set; }

    public Prisoner Prisoner { get; set; }
    public CallHistory CallRequest { get; set; }
}
