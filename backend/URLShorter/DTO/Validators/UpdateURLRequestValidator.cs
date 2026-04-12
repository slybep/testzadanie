using FluentValidation;

namespace URLShorter.DTO.Validators
{
    public class UpdateURLRequestValidator : AbstractValidator<UpdateUrlRequest>
    {
        public UpdateURLRequestValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("URL is required")
                .NotNull().WithMessage("URL cannot be null")
                .MaximumLength(100).WithMessage("URL cannot exceed 100 characters")
                .Must(BeValidUrl).WithMessage("URL must start with http:// or https://");
        }
        private bool BeValidUrl(string url)
        {
            return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }
    }
}
