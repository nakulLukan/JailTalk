using JailTalk.Shared;

namespace JailTalk.Domain.Prison;
public class JailAccountRechargeRequest
{
    public long Id { get; set; }
    public int JailId { get; set; }

    /// <summary>
    /// Hash value of guid
    /// </summary>
    public string RechargeSecretHash { get; set; }
    public float RechargeAmount { get; set; }
    public string RequestedBy { get; set; }
    public DateTimeOffset RequestedOn { get; set; }
    public JailAccountRechargeRequestStatus RequestStatus { get; set; }
    public DateTimeOffset? RequestCompletedOn { get; set; }

    /// <summary>
    /// If not null then any request after this value should be rejected.
    /// </summary>
    public DateTimeOffset? ExpiresOn { get; set; }

    /// <summary>
    /// Number of times the secret failed to match.
    /// </summary>
    public short RetryCount { get; set; }

    public Jail Jail { get; set; }
}
