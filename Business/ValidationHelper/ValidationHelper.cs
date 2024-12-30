using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public static class ValidationHelper
{
    public static bool ValidateModel<T>(T model, out string validationMessage)
    {
        var validationContext = new ValidationContext(model);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);
        validationMessage = validationResults.FirstOrDefault()?.ErrorMessage;
        return isValid;
    }
}