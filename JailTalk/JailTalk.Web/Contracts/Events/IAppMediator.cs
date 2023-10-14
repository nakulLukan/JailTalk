using JailTalk.Shared.Models;
using MediatR;

namespace JailTalk.Web.Contracts.Events;
public interface IAppMediator
{
    public Task<ResponseDto<TData>> Send<TData>(IRequest<ResponseDto<TData>> request, bool showLoader = false);
    public Task<ResponseDto<TResponse>> Send<TResponse>(IRequest<TResponse> request, bool showLoader = false);
}