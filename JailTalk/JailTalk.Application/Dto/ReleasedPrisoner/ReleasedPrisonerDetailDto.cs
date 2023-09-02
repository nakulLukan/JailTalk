namespace JailTalk.Application.Dto.ReleasedPrisoner;

public class ReleasedPrisonerDetailDto
{
    public Guid Id { get; set; }
    public string Pid { get; set; }
    public string LastJailName { get; set; }
    public string LastJailCode { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string AddressAsText { get; set; }
    public string LastReleasedOnAsText { get; set; }
}
