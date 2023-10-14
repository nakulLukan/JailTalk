using JailTalk.Application.Requests.Jail;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JailTalk.Api.Controllers.v1;
public class PrisonAccountController : AppBaseController
{
    public PrisonAccountController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("prison-account/recharge")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.Unauthorized)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> RechargeAccountAction(RechargeJailAccountCommand request)
    {
        var response = await Mediator.Send(request);

        return Ok(response);
    }
}
