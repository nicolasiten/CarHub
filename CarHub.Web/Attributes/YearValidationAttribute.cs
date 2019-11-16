using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarHub.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class YearValidationAttribute : ValidationAttribute
    {
        public YearValidationAttribute(int minYear)
        {
            MinYear = minYear;
        }

        private int MinYear { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validation)
        {
            int yearValue = (int)value;

            if (yearValue >= MinYear && yearValue <= DateTime.Now.Year)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Year must be between {MinYear} and {DateTime.Now.Year}");
            }
        }
    }
}
