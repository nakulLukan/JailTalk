using Fluxor;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Requests.Prisoner;
using JailTalk.Web.Contracts.Events;

namespace JailTalk.Web.Store.Prisoner;

public class PrisonerSecondaryDetailFeature
{
    public record GetSecondaryDetailAction(Guid PrisonerId);

    public class GetSecondaryDetailEffect : Effect<GetSecondaryDetailAction>
    {
        readonly IAppMediator _mediator;
        readonly IDispatcher _dispatcher;

        public GetSecondaryDetailEffect(IAppMediator mediator, IDispatcher dispatcher)
        {
            _mediator = mediator;
            _dispatcher = dispatcher;
        }

        public override async Task HandleAsync(GetSecondaryDetailAction action, IDispatcher dispatcher)
        {
            var result = await _mediator.Send(new GetPrisonerSecondaryDetailsQuery
            {
                PrisonerId = action.PrisonerId,
            });

            _dispatcher.Dispatch(new SetSecondaryDetailAction(result.Data));
        }
    }

    public record SetSecondaryDetailAction(PrisonerSecondaryDetailDto Data);
    public record UpdateIsUnlimitedCallEnabledAction(bool IsEnabled);
    public record UpdateIsReleasedAction(bool IsReleased);

    public static class Reducers
    {
        [ReducerMethod]
        public static PrisonerSecondaryDetailState ReduceSetSecondaryDetailAction(PrisonerSecondaryDetailState state, SetSecondaryDetailAction action)
        {
            return new PrisonerSecondaryDetailState()
            {
                IsBlocked = action.Data.IsBlocked,
                IsUnlimitedCallEnabled = action.Data.IsUnlimitedCallEnabled,
                IsPrisonerReleased = action.Data.IsReleased,
                RestrictionEndsOn = action.Data.RetrictionEndsOn,
                PrisonerId = action.Data.PrisonerId,
                Pid = action.Data.Pid
            };
        }

        [ReducerMethod]
        public static PrisonerSecondaryDetailState ReduceUpdateIsUnlimitedCallEnabledAction(PrisonerSecondaryDetailState state, UpdateIsUnlimitedCallEnabledAction action)
        {
            return state with
            {
                IsUnlimitedCallEnabled = action.IsEnabled,
            };
        }

        [ReducerMethod]
        public static PrisonerSecondaryDetailState ReduceUpdateIsReleasedAction(PrisonerSecondaryDetailState state, UpdateIsReleasedAction action)
        {
            return state with
            {
                IsPrisonerReleased = action.IsReleased,
            };
        }
    }
}