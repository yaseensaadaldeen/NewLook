using System.ComponentModel.DataAnnotations;

namespace NEWLOOK.Models
{
    public class BirthDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime birthDate)
            {
                return birthDate <= DateTime.Today;
            }
            return false;
        }
    }
}
