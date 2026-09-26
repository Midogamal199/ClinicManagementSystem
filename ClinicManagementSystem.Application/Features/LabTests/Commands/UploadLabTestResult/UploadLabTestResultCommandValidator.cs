using FluentValidation;

namespace ClinicManagementSystem.Application.Features.LabTests.Commands.UploadLabTestResult
{
    public class UploadLabTestResultCommandValidator : AbstractValidator<UploadLabTestResultCommand>
    {
        private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        public UploadLabTestResultCommandValidator()
        {
            RuleFor(x => x.LabTestId).NotEmpty();

            RuleFor(x => x.ResultFile)
                .NotNull().WithMessage("Result file is required.");

            RuleFor(x => x.ResultFile)
                .Must(f => f.Length > 0 && f.Length <= MaxFileSizeBytes)
                .WithMessage("File size must be between 1 byte and 10 MB.")
                .When(x => x.ResultFile is not null);

            RuleFor(x => x.ResultFile)
                .Must(f => AllowedExtensions.Contains(System.IO.Path.GetExtension(f.FileName).ToLower()))
                .WithMessage($"Allowed file types are: {string.Join(", ", AllowedExtensions)}.")
                .When(x => x.ResultFile is not null);
        }
    }
}