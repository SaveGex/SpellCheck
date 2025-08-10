using Infrastructure.Models.ModelsDTO;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequireNumberOrEmail : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is UserCreateDTO dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Number) || !string.IsNullOrWhiteSpace(dto.Email))
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
