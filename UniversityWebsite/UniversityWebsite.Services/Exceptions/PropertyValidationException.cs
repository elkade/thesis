using System;

namespace UniversityWebsite.Services.Exceptions
{
    public class PropertyValidationException : Exception
    {
        public string PropertyName { get; set; }
        public string PropertyValidationMessage { get; set; }

        public PropertyValidationException(string propertyName, string propertyValidationMessage)
        {
            PropertyName = propertyName;
            PropertyValidationMessage = propertyValidationMessage;
        }
    }
}
