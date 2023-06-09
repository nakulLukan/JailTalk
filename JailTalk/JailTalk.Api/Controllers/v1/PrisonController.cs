﻿using JailTalk.Api.Filters;
using JailTalk.Application.Dto.Prison;
using JailTalk.Application.Requests.Identity;
using JailTalk.Application.Requests.Prison;
using JailTalk.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JailTalk.Api.Controllers.v1;

[Authorize]
public class PrisonController : AppBaseController
{
    public PrisonController(IMediator mediator) : base(mediator)
    {
    }

    [RequestSizeLimit(5_242_880)]
    [HttpPost("prisoners/validate")]
    [ProducesResponseType(typeof(ResponseDto<string>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AuthenticatePrisoner(IFormFile faceImageBinary, [FromForm]
        string pid)
    {
        // Save the file to the specified path
        using (var memStream = new MemoryStream())
        {
            await faceImageBinary.CopyToAsync(memStream);

            var data = await Mediator.Send(new PrisonerTokenQuery
            {
                Pid = pid,
                FaceImageData = memStream.ToArray()
            });
            return Ok(data);
        }
    }

    [HttpGet("prisoners/contacts/all")]
    [ProducesResponseType(typeof(List<ShowContactsDto>), (int)HttpStatusCode.OK)]
    [SessionAuthFilter]
    public async Task<IActionResult> GetPrionerContactNumbers()
    {
        var data = await Mediator.Send(new PrisonerAllActiveContactsQuery());
        return Ok(data);
    }

    [HttpPost("prisoners/contacts/begin-call")]
    [ProducesResponseType(typeof(RequestCallResultDto), (int)HttpStatusCode.OK)]
    [SessionAuthFilter]
    public async Task<IActionResult> RequestToBeginCall([FromBody] RequestCallCommand request)
    {
        var data = await Mediator.Send(request);
        return Ok(data);
    }

    [HttpPut("prisoners/contacts/end-call")]
    [ProducesResponseType(typeof(EndCallResultDto), (int)HttpStatusCode.OK)]
    [SessionAuthFilter]
    public async Task<IActionResult> RequestToBeginCall([FromBody] EndCallCommmand request)
    {
        var data = await Mediator.Send(request);
        return Ok(data);
    }
}
