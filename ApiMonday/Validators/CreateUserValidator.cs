using FluentValidation;
using ApiMonday.DTOs.User;

namespace InventoryAPI.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Användarnamn är obligatoriskt.")
            .MaximumLength(50).WithMessage("Användarnamn får max vara 50 tecken.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-post är obligatorisk.")
            .EmailAddress().WithMessage("Ange en giltig e-postadress.");
    }
}