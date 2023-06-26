using JailTalk.Application.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JailTalk.Api.Controllers.v1;

[Authorize]
public class PrisonController : AppBaseController
{
    public PrisonController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("prisons/{prisonId}/{prisoner}/validate")]
    public async Task<IActionResult> AuthenticatePrisoner()
    {
        var data = await Mediator.Send(new JwtTokenRequest());
        return Ok(data);
    }
}
