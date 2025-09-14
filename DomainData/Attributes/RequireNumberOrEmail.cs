using DomainData.Models;
using System.ComponentModel.DataAnnotations;

namespace DomainData.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireNumberOrEmail : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Number) || !string.IsNullOrWhiteSpace(user.Email))
            {
                return ValidationResult.Success;
            }
        }

        return CreateFailedValidationResult(validationContext);
    }

    private protected ValidationResult CreateFailedValidationResult(ValidationContext validationContext)
    {
        string[]? memberNames = validationContext.MemberName is { } memberName
            ? new[] { memberName }
            : null;

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
    }
}
