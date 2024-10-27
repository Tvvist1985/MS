using System.ComponentModel.DataAnnotations;

namespace PET.Services.Validator
{
    public class AgeValidationAttribute : ValidationAttribute
    {
        private string _errorMessage;
        private byte _minAge;
        private byte _maxAge;

        public AgeValidationAttribute(byte Min, byte Max) => (_minAge, _maxAge, _errorMessage)
            = (Min, Max, $"Возраст меньше{_minAge} или превышает {_maxAge}");
       
        public AgeValidationAttribute(byte Min, byte Max, string ErrorMessage ) => (_minAge, _maxAge, _errorMessage)
            = (Min, Max, ErrorMessage);
                
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime && !GetValidationAge(dateTime))                          
                return new ValidationResult(_errorMessage, new []{ validationContext.MemberName});
            
            return ValidationResult.Success;
        }

        private bool GetValidationAge(DateTime dateTime)
        {            
            DateTime nowDate = DateTime.Today;
            byte age = (byte)(nowDate.Year - dateTime.Year);
            if (nowDate > nowDate.AddYears(-age))            
                age--;
            
            if (age < _minAge || age > _maxAge)
                return false;

            return true;
        }
    }
}
