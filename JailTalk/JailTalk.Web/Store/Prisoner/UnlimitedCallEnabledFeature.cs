using Fluxor;
using JailTalk.Application.Requests.Prison;
using JailTalk.Web.Contracts.Events;

namespace JailTalk.Web.Store.Prisoner;

public class UnlimitedCallEnabledFeature
{
    public record GetIsEnabledAction(Guid PrisonerId);

    public class GetIsEnabledEffect : Effect<GetIsEnabledAction>
    {
        readonly IAppMediator _mediator;
        readonly IDispatcher _dispatcher;

        public GetIsEnabledEffect(IAppMediator mediator, IDispatcher dispatcher)
        {
            _mediator = mediator;
            _dispatcher = dispatcher;
        }

        public override async Task HandleAsync(GetIsEnabledAction action, IDispatcher dispatcher)
        {
            var result = await _mediator.Send(new UnlimitedCallPriviledgeEnabledQuery
            {
                PrisonerId = action.PrisonerId,
            });

            _dispatcher.Dispatch(new SetIsEnabledAction(result.Data));
        }
    }

    public record SetIsEnabledAction(bool IsUnlimitedCallEnabled);

    public static class Reducers
    {
        [ReducerMethod]
        public static UnlimitedCallEnabledState ReduceGetUnlimitedCallEnabledAction(UnlimitedCallEnabledState state, SetIsEnabledAction action)
        {
            return state with
            {
                IsEnabled = action.IsUnlimitedCallEnabled
            };
        }
    }
}