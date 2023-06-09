﻿using FluentValidation;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using JailTalk.Web.Contracts.Events;
using MediatR;
using Serilog;

namespace JailTalk.Web.Impl.Events;

public class AppMediator : IAppMediator
{
    private readonly IMediator mediator;
    private readonly ILogger<AppMediator> logger;

    public AppMediator(IMediator mediator, ILogger<AppMediator> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    public async Task<ResponseDto<TData>> Send<TData>(IRequest<ResponseDto<TData>> request)
    {
        try
        {
            return await mediator.Send(request);
        }
        catch (ValidationException ex)
        {
            return new ResponseDto<TData>(ex.Errors.Select(x => new FieldErrorDto(x.PropertyName, x.ErrorMessage)));
        }
        catch (AppException ex)
        {
            return new ResponseDto<TData>(new ErrorDto(ex.ErrorMessage));
        }
        catch (Exception ex)
        {
            logger.LogError("{0} mediator request failed.", request.GetType().Name);
            logger.LogError("Exception: {message}\nStackTrace: {stackTrace}", ex.Message, ex.StackTrace);
            return new ResponseDto<TData>(new ErrorDto("Oops, something went wrong."));
        }
    }

    public async Task<ResponseDto<TResponse>> Send<TResponse>(IRequest<TResponse> request)
    {
        try
        {
            return new ResponseDto<TResponse>(await mediator.Send(request));
        }
        catch (ValidationException ex)
        {
            Log.Logger.Error("Validation exception, Message: {message}\nStack: {stack}", ex.Message, ex.StackTrace);
            return new ResponseDto<TResponse>(new FormError(ex.Errors));
        }
        catch (AppException ex)
        {
            Log.Logger.Error("Validation exception, Message: {message}\nStack: {stack}", ex.Message, ex.StackTrace);
            return new ResponseDto<TResponse>(new ErrorDto(ex.ErrorMessage));
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Validation exception, Message: {message}\nStack: {stack}", ex.Message, ex.StackTrace);
            return new ResponseDto<TResponse>(new ErrorDto("Oops, something went wrong."));
        }
    }
}