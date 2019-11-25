using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace CarHub.Core.Entities.Validations
{
    public class RepairValidator : AbstractValidator<Repair>
    {
        public RepairValidator()
        {
            RuleFor(r => r.RepairDescription)
                .NotNull()
                .NotEmpty();

            RuleFor(r => r.RepairCost)
                .GreaterThanOrEqualTo(0);
        }
    }
}
