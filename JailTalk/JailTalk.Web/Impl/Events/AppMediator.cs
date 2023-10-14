using FluentValidation;
using Fluxor;
using JailTalk.Shared.Models;
using JailTalk.Shared.Utilities;
using JailTalk.Web.Contracts.Events;
using JailTalk.Web.Store.Http;
using MediatR;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace JailTalk.Web.Impl.Events;

public class AppMediator : IAppMediator
{
    private readonly IMediator mediator;
    private readonly ILogger<AppMediator> logger;
    private readonly NavigationManager _navigationManager;
    private readonly IDispatcher _dispatcher;

    public AppMediator(IMediator mediator, ILogger<AppMediator> logger, NavigationManager navigationManager, IDispatcher dispatcher)
    {
        this.mediator = mediator;
        this.logger = logger;
        _navigationManager = navigationManager;
        _dispatcher = dispatcher;
    }

    public async Task<ResponseDto<TData>> Send<TData>(IRequest<ResponseDto<TData>> request, bool showLoader = false)
    {
        if (showLoader)
        {
            _dispatcher.Dispatch(new HttpCallRequestFeature.WaitForHttpResponseAction());
        }
        try
        {
            return await mediator.Send(request);
        }
        catch (ValidationException ex)
        {
            return new ResponseDto<TData>(new FormError(ex.Errors));
        }
        catch (AppException ex)
        {
            logger.LogError(ex, "Mediator failed for request {request}", request.GetType().Name);
            NavigateIfInAccessable(ex);
            return new ResponseDto<TData>(new ErrorDto(ex.ErrorMessage));
        }
        catch (Exception ex)
        {
            logger.LogError("{0} mediator request failed.", request.GetType().Name);
            logger.LogError("Exception: {message}\nStackTrace: {stackTrace}", ex.Message, ex.StackTrace);
            return new ResponseDto<TData>(new ErrorDto("Oops, something went wrong."));
        }
        finally
        {
            if (showLoader)
            {
                _dispatcher.Dispatch(new HttpCallRequestFeature.HttpResponseRecievedAction());
            }
        }
    }

    private void NavigateIfInAccessable(AppException ex)
    {
        if (ex.IsInAccessible)
        {
            _navigationManager.NavigateTo("/unauthorized");
        }
    }

    public async Task<ResponseDto<TResponse>> Send<TResponse>(IRequest<TResponse> request, bool showLoader = false)
    {
        try
        {
            if (showLoader)
            {
                _dispatcher.Dispatch(new HttpCallRequestFeature.WaitForHttpResponseAction());
            }
            return new ResponseDto<TResponse>(await mediator.Send(request));
        }
        catch (ValidationException ex)
        {
            Log.Logger.Error("Validation exception, Message: {message}\nStack: {stack}", ex.Message, ex.StackTrace);
            return new ResponseDto<TResponse>(new FormError(ex.Errors));
        }
        catch (AppException ex)
        {
            logger.LogError(ex, "Mediator failed for request {request}", request.GetType().Name);
            NavigateIfInAccessable(ex);
            return new ResponseDto<TResponse>(new ErrorDto(ex.ErrorMessage));
        }
        catch (Exception ex)
        {
            Log.Logger.Error("Validation exception, Message: {message}\nStack: {stack}", ex.Message, ex.StackTrace);
            return new ResponseDto<TResponse>(new ErrorDto("Oops, something went wrong."));
        }
        finally
        {
            if (showLoader)
            {
                _dispatcher.Dispatch(new HttpCallRequestFeature.HttpResponseRecievedAction());
            }
        }
    }
}