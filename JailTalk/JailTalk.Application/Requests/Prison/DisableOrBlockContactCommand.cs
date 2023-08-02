using JailTalk.Application.Contracts.Data;
using JailTalk.Application.Contracts.Http;
using JailTalk.Domain.Prison;
using JailTalk.Shared.Extensions;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JailTalk.Application.Requests.Prison;

public class DisableOrBlockContactCommand : IRequest<ResponseDto<bool>>
{
    public long ContactId { get; set; }
    public bool? BlockContact { get; set; }
    public bool? DisableContact { get; set; }
}
public class DisableOrBlockContactCommandHandler : IRequestHandler<DisableOrBlockContactCommand, ResponseDto<bool>>
{
    readonly IAppDbContext _dbContext;
    readonly IAppRequestContext _requestContext;
    readonly ILogger<DisableOrBlockContactCommandHandler> _logger;
    readonly IApplicationSettingsProvider _settingsProvider;

    public DisableOrBlockContactCommandHandler(IAppDbContext dbContext, IAppRequestContext requestContext, ILogger<DisableOrBlockContactCommandHandler> logger, IApplicationSettingsProvider settingsProvider)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
        _logger = logger;
        _settingsProvider = settingsProvider;
    }

    public async Task<ResponseDto<bool>> Handle(DisableOrBlockContactCommand request, CancellationToken cancellationToken)
    {
        var prisonId = _requestContext.GetAssociatedPrisonId();
        var contact = await _dbContext.PhoneDirectory.AsTracking()
            .WhereInPrison(x => x.Prisoner.JailId, prisonId)
            .FirstOrDefaultAsync(x => x.Id == request.ContactId, cancellationToken);

        if (contact == null)
        {
            _logger.LogError("Either contact number is not present in the system or the user belongs to a different prison.");
            throw new AppException("Unknown contact id.");
        }

        if (request.BlockContact.HasValue)
        {
            contact.IsBlocked = request.BlockContact.Value;
        }

        if (request.DisableContact.HasValue)
        {
            await ValidateIfActiveContactsIsInLimit(request, contact, cancellationToken);
            contact.IsActive = !request.DisableContact.Value;
        }

        _logger.LogInformation("Contact {contact} 'is active: {active}', 'is disabled: {disabled}'", contact.Id, contact.IsActive, contact.IsBlocked);
        await _dbContext.SaveAsync(cancellationToken);
        return new(true);
    }

    private async Task ValidateIfActiveContactsIsInLimit(DisableOrBlockContactCommand request, PhoneDirectory contact, CancellationToken cancellationToken)
    {
        if (!request.DisableContact.Value)
        {
            var activeContactsCount = await _dbContext.PhoneDirectory
                .Where(x => x.PrisonerId == contact.PrisonerId
                    && x.IsActive)
                .CountAsync(cancellationToken);
            var maxAllowedActiveContactsCount = await _settingsProvider.GetMaxContactNumbersInActiveState();
            if (activeContactsCount >= maxAllowedActiveContactsCount)
            {
                throw new AppException($"Cannot enable more than {maxAllowedActiveContactsCount} contacts. Disable atleast 1 contact.");
            }
        }
    }
}

