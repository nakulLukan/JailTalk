using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JailTalk.Api.Controllers.v1;

public class TestController : AppBaseController
{
    public TestController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("test")]
    public async Task<IActionResult> GetTest()
    {
        return Ok("unprotected api accessable");
    }

    [Authorize]
    [HttpGet("test-athuorized")]
    public async Task<IActionResult> GetTestAuthorized()
    {
        return Ok("protected api accessable");
    }
}
