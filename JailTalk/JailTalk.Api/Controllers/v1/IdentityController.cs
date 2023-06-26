using JailTalk.Application.Requests.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JailTalk.Api.Controllers.v1;

public class IdentityController : AppBaseController
{
    public IdentityController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("validate")]
    public async Task<IActionResult> ValidateDevice([FromQuery] string macAddress, [FromQuery] Guid deviceSecret)
    {
        var data = await Mediator.Send(new JwtTokenRequest()
        {
            MacAddress = macAddress,
            DeviceSecretIdentifier = deviceSecret
        });
        return Ok(data);
    }
}
