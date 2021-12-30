using VBMS.Application.Features.BrandTests.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace VBMS.Application.Validators.Features.BrandTests.Commands.AddEdit
{
    public class AddEditBrandTestCommandValidator : AbstractValidator<AddEditBrandTestCommand>
    {
        public AddEditBrandTestCommandValidator(IStringLocalizer<AddEditBrandTestCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            RuleFor(request => request.Tax)
                .GreaterThan(0).WithMessage(x => localizer["Tax must be greater than 0"]);
        }
    }
}