using System.Globalization;
using System.Windows.Controls;

namespace SimpleNotepad.View.Dialog.ValidationRules
{
    public class StringValidationRuleNonNullOrWhitespace : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Must provide a string value");

            var str = value as string;

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Must provide a string value");

            return ValidationResult.ValidResult;
        }
    }
}
