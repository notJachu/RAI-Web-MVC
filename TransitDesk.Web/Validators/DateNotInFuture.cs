using System.ComponentModel.DataAnnotations;

namespace TransitDesk.Web.Validators;

public class DateNotInFutureAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateTimeValue)
        {
            if (dateTimeValue > DateTime.Now)
            {
                var memberNames = validationContext.MemberName != null ? new[] { validationContext.MemberName } : null;
                return new ValidationResult(ErrorMessage ?? "Data nie może być z przyszłości.", memberNames);
            }
        }

        return ValidationResult.Success;
    }
}