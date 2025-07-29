using FluentValidation;

namespace Adly.Application.Common.Validation;

/// <summary>
/// Marker class for validation models.
/// </summary>
/// <typeparam name="TRequestModel">A request model in form pf commend or query</typeparam>
public class ValidationModelBase<TRequestModel>:AbstractValidator<TRequestModel> where TRequestModel : class
{
    
}