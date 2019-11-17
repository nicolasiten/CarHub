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
            int maxYear = DateTime.Now.Year + 1;

            if (yearValue >= MinYear && yearValue <= maxYear)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Year must be between {MinYear} and {maxYear}");
            }
        }
    }
}
