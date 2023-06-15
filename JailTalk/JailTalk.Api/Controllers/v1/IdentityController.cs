using JailTalk.Application.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JailTalk.Api.Controllers.v1;

public class IdentityController : AppBaseController
{

    public IdentityController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("token")]
    public async Task<IActionResult> GetToken()
    {
        var data = await Mediator.Send(new JwtTokenRequest());
        return Ok(data);
    }
}
