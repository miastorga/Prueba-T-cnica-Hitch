using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AsientosContrablesApi.Models;

namespace AsientosContrablesApi.Validations
{
  public class ValidateDebitCreditTotalAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value is List<JournalEntryLinesDTO> lines)
      {
        double totalDebit = 0;
        double totalCredit = 0;

        foreach (var line in lines)
        {
          totalDebit += line.Debit;
          totalCredit += line.Credit;
        }

        if (Math.Abs(totalDebit - totalCredit) < 0.0001) // Usar tolerancia para manejar errores de redondeo en números de punto flotante
        {
          return ValidationResult.Success;
        }
      }

      return new ValidationResult(ErrorMessage);
    }
  }
}
