namespace JailTalk.Domain.Prison;
public class JailAccountBalance
{
    public int Id { get; set; }
    public int JailId { get; set; }
    public float BalanceAmount { get; set; }

    public Jail Jail { get; set; }
}
