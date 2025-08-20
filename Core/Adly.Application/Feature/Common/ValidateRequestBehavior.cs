using Adly.Application.Common;
using Adly.Application.Extensions;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Common;

public class ValidateRequestBehavior<TRequest, TResponse>(IValidator<TRequest> validator) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOperatorResult, new()
{
    // public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    // {
    //     var validationResult = await validator.ValidateAsync(message, cancellationToken);
    //     if (!validationResult.IsValid)
    //     {
    //         return new TResponse()
    //         {
    //             IsNotFound = false,
    //             IsSuccess = false,
    //             ErrorMessages = validationResult.Errors.ConvertToKeyValuePair()
    //         };
    //     }
    //     return await next(message, cancellationToken);
    // }


    public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(message, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new TResponse()
            {
                IsNotFound = false,
                IsSuccess = false,
                ErrorMessages = validationResult.Errors.ConvertToKeyValuePair()
            };
        }
        return await next(message, cancellationToken); ;
    }
}
