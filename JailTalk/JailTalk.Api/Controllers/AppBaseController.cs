using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JailTalk.Api.Controllers;

[ApiController]
[Route("api")]
public class AppBaseController : ControllerBase
{
    protected readonly IMediator Mediator;

    public AppBaseController(IMediator mediator)
    {
        Mediator = mediator;
    }
}
