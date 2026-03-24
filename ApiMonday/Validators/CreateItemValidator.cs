using FluentValidation;
using ApiMonday.DTOs.Item;

namespace ApiMonday.Validators;

public class CreateItemValidator : AbstractValidator<CreateItemDto>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Namn är obligatoriskt.")
            .MaximumLength(100).WithMessage("Namn får max vara 100 tecken.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Mängd måste vara större än 0.");

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Utgångsdatum måste vara i framtiden.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Kategori måste anges.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("Användare måste anges.");
    }
}