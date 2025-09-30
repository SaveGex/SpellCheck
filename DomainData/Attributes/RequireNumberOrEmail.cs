using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DomainData.Attributes;

/// <summary>
/// Can use any <see cref="TModel"/> which contains properties "Number" and "Email". <br/>
/// </summary>
/// <typeparam name="TModel">Type of Model which will be checks on number or email is not null</typeparam>
[AttributeUsage(AttributeTargets.Class)]
public class RequireNumberOrEmail<TModel> : ValidationAttribute
{
    public Type ModelType { get; init; } = typeof(TModel);
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value is TModel model)
        {
            PropertyInfo numberProp = model.GetType().GetProperty("Number")
                ?? throw new InvalidOperationException("Model does not contain a 'Number' property.");
            PropertyInfo emailProp = model.GetType().GetProperty("Email")
                ?? throw new InvalidOperationException("Model does not contain an 'Email' property.");
            if (
                !string.IsNullOrWhiteSpace(numberProp.GetValue(model)?.ToString()) || !string.IsNullOrWhiteSpace(emailProp.GetValue(model)?.ToString()))
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
