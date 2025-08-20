using Adly.Application.Common;
using Adly.Application.Feature.Common;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Adly.Application.Tests;

public static class Helpers
{
    public static  ValueTask<TResponse> ValidateAndExecuteAsync<TRequest,TResponse>(TRequest request,IRequestHandler<TRequest, TResponse> handler,IServiceProvider serviceProvider)
        where TRequest : IRequest<TResponse> where TResponse :IOperatorResult,new ()
    {
        var validationBehavior = new ValidateRequestBehavior<TRequest, TResponse>(
            serviceProvider.GetRequiredService<IValidator<TRequest>>());
        return  validationBehavior.Handle(request, handler.Handle,CancellationToken.None );
    }



}
