using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Category.Commands;

public record CreateCategoryCommand(string CategoryName)
    : IRequest<OperationResult<bool>>, IValidatableModel<CreateCategoryCommand>
{
    public IValidator<CreateCategoryCommand> Validate(ValidationModelBase<CreateCategoryCommand> validator)
    {

        validator.RuleFor(x => x.CategoryName).NotEmpty();
        return validator;

    }
}