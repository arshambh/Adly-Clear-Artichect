using Adly.Application.Common;
using Adly.Application.Common.Validation;
using FluentValidation;
using Mediator;

namespace Adly.Application.Feature.Ad.Commands;

public record CreateAdCommand(Guid UserId, Guid CategoryId, Guid LocationId, string Title, string Description, CreateAdCommand.CreateAdImagesModel[] AdImages)
    :IRequest<OperationResult<bool>>, IValidatableModel<CreateAdCommand>
{
    public record CreateAdImagesModel(string Base64File, string FileContent);

    public IValidator<CreateAdCommand> Validate(ValidationModelBase<CreateAdCommand> validator)
    {
       validator.RuleFor(x => x.UserId)
            .NotEmpty();

       validator.RuleFor(x => x.CategoryId)
           .NotEmpty();

       validator.RuleFor(x => x.LocationId)
           .NotEmpty();


       validator.RuleFor(x => x.Title)
           .NotEmpty();

       validator.RuleFor(x => x.Description)
           .NotEmpty();

       return validator;
    }
}


