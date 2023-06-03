namespace JailTalk.Domain.Prison;
public class PhoneBalance
{
    public int Id { get; set; }
    public Guid PrisonerId { get; set; }
    public float Balance { get; set; }
    public DateTimeOffset LastUpdatedOn { get; set; }

    public Prisoner Prisoner { get; set; }
}
