namespace JailTalk.Application.Dto.Jail;

public class PrisonListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public bool IsSystemTurnedOff { get; set; }
    public string AddressAsText { get; set; }
}
