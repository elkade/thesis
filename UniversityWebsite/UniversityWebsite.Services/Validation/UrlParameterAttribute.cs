using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UniversityWebsite.Services.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class UrlParameterAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Wyrażenie zawiera niepoprawne znaki.";

        public UrlParameterAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessageString;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            Regex regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9\-][a-zA-Z0-9]*$");
            Match match = regex.Match(value.ToString());
            if (match.Success)
                return ValidationResult.Success;
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }

}