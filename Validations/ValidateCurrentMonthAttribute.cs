using System.ComponentModel.DataAnnotations;

namespace AsientosContrablesApi.Validations
{
  public class CurrentMonthAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      DateTime date = (DateTime)value;

      if (date.Year < DateTime.Now.Year || (date.Year == DateTime.Now.Year && date.Month <= DateTime.Now.Month))
      {
        return ValidationResult.Success;
      }

      return new ValidationResult(ErrorMessage);
    }
  }
}