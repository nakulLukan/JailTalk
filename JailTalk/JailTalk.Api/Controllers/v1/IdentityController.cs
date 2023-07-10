using JailTalk.Application.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JailTalk.Api.Controllers.v1;

public class IdentityController : AppBaseController
{
    public IdentityController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("devices/validate")]
    public async Task<IActionResult> ValidateDevice([FromBody] DeviceTokenQuery request)
    {
        var data = await Mediator.Send(request);
        return Ok(data);
    }
}
