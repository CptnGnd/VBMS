using VBMS.Application.Features.ProductTests.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace VBMS.Application.Validators.Features.ProductTests.Commands.AddEdit
{
    public class AddEditProductTestCommandValidator : AbstractValidator<AddEditProductTestCommand>
    {
        public AddEditProductTestCommandValidator(IStringLocalizer<AddEditProductTestCommandValidator> localizer)
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Name is required!"]);
            RuleFor(request => request.Barcode)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Barcode is required!"]);
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Description is required!"]);
            RuleFor(request => request.BrandTestId)
                .GreaterThan(0).WithMessage(x => localizer["BrandTest is required!"]);
            RuleFor(request => request.Rate)
                .GreaterThan(0).WithMessage(x => localizer["Rate must be greater than 0"]);
        }
    }
}