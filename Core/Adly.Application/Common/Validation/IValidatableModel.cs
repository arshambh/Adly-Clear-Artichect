using FluentValidation;

namespace Adly.Application.Common.Validation;

public interface IValidatableModel<TRequestApplicationModel> where TRequestApplicationModel : class
{
    IValidator<TRequestApplicationModel> Validate(ValidationModelBase<TRequestApplicationModel> validator);
}