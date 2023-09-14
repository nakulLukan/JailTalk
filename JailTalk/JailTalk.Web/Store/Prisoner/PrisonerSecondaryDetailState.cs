using Fluxor;

namespace JailTalk.Web.Store.Prisoner;
[FeatureState]
public record PrisonerSecondaryDetailState
{
    public bool IsUnlimitedCallEnabled { get; init; }
    public bool IsBlocked { get; init; }
    public DateTimeOffset? RestrictionEndsOn { get; init; }
    public bool IsPrisonerReleased { get; init; }
    public Guid PrisonerId { get; set; }
    public string Pid { get; set; }
}