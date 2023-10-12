using JailTalk.Application.Dto.Device;
using JailTalk.Application.Requests.Device;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JailTalk.Api.Controllers.v1;

[Authorize]
public class DeviceController : AppBaseController
{
    public DeviceController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Device basic details such as device code, prison name and prison code.
    /// </summary>
    /// <returns></returns>
    [HttpGet("devices/basic-details")]
    [ProducesResponseType(typeof(DeviceBasicDetailDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDeviceBasicDetail()
    {
        var data = await Mediator.Send(new GetDeviceBasicDetailQuery());
        return Ok(data);
    }
}
