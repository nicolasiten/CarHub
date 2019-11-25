using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FluentValidation;

namespace CarHub.Core.Entities.Validations
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(c => c.Vin)
                .NotEmpty()
                .MinimumLength(17)
                .MaximumLength(17);

            RuleFor(c => c.Year)
                .GreaterThan(1990);

            RuleFor(c => c.Make)
                .NotEmpty();

            RuleFor(c => c.Model)
                .NotEmpty();

            RuleFor(c => c.Trim)
                .NotEmpty();

            RuleFor(c => c.Description)
                .NotEmpty();

            RuleFor(c => c.Kilometers)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.TransmissionType)
                .NotNull();

            RuleFor(c => c.PurchaseDate)
                .NotNull();

            RuleFor(c => c.LotDate)
                .NotNull()
                .GreaterThanOrEqualTo(c => c.PurchaseDate);

            RuleFor(c => c.SellingPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(c => c.ShowCase)
                .NotNull();

            RuleFor(c => c.ThumbnailImage)
                .SetValidator(new FileDataValidator<Thumbnail>())
                .When(c => c.ThumbnailImage != null);

            RuleForEach(c => c.Images)
                .SetValidator(new FileDataValidator<Image>())
                .When(c => c.Images != null);

            RuleForEach(c => c.Repairs)
                .SetValidator(new RepairValidator())
                .When(c => c.Repairs != null);
        }
    }
}
