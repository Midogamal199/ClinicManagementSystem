using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Attendances.Queries.GetAllAttendances
{
    public class GetAllAttendancesQueryValidator : AbstractValidator<GetAllAttendancesQuery>
    {
        public GetAllAttendancesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        }
    }
}