using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Search.Queries.GlobalSearch
{
    public class GlobalSearchQueryValidator : AbstractValidator<GlobalSearchQuery>
    {
        public GlobalSearchQueryValidator()
        {
            RuleFor(x => x.Term)
                .NotEmpty().WithMessage("Search term is required.")
                .Must(t => t.Trim().Length >= 2).WithMessage("Search term must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Search term must not exceed 100 characters.");

            RuleFor(x => x.Limit)
                .InclusiveBetween(1, 20).WithMessage("Limit must be between 1 and 20.");
        }
    }
}