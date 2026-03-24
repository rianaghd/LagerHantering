using FluentValidation;
using ApiMonday.DTOs.Category;

namespace InventoryAPI.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategorinamn är obligatoriskt.")
            .MaximumLength(50).WithMessage("Kategorinamn får max vara 50 tecken.");
    }
}