using JailTalk.Application.Requests.Identity;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JailTalk.Api.Controllers.v1;

public class IdentityController : AppBaseController
{
    public IdentityController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Authenticates a device
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("devices/validate")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> ValidateDevice([FromBody] DeviceTokenQuery request)
    {
        var data = await Mediator.Send(request);
        return Ok(data);
    }
}
