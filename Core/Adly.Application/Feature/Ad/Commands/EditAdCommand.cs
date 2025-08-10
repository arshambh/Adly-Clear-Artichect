using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Ad.Commands;

public record EditAdCommand(
    Guid AdId,
    Guid? LocationId,
    Guid? CategoryId,
    string? Title,
    string? Description,
    string[] RemovedImageNames,
    EditAdCommand.AddNewImagesModel[] NewImages):IRequest<OperationResult<bool>>,IValidatableModel<EditAdCommand>
{
    public record AddNewImagesModel(string ImageContent, string ImageType);

    public IValidator<EditAdCommand> Validate(ValidationModelBase<EditAdCommand> validator)
    {
        validator.RuleFor(x => x.AdId)
            .NotEmpty();
        return validator;
    }
}