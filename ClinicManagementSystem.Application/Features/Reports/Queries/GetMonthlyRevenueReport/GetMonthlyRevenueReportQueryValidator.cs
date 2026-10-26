using System;
using FluentValidation;

namespace ClinicManagementSystem.Application.Features.Reports.Queries.GetMonthlyRevenueReport
{
    public class GetMonthlyRevenueReportQueryValidator : AbstractValidator<GetMonthlyRevenueReportQuery>
    {
        public GetMonthlyRevenueReportQueryValidator()
        {
            RuleFor(x => x.Year)
                .InclusiveBetween(2020, 2100).WithMessage("Year must be between 2020 and 2100.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x)
                .Must(x => new DateTime(x.Year, x.Month, 1) <=
                           new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1))
                .WithMessage("Cannot export a report for a future month.")
                .When(x => x.Year is >= 2020 and <= 2100 && x.Month is >= 1 and <= 12);
        }
    }
}