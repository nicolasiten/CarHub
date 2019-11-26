using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace CarHub.Core.Entities.Validations
{
    public class FileDataValidator<T> : AbstractValidator<T> where T : FileData
    {
        public FileDataValidator()
        {
            RuleFor(f => f.ImageType)
                .NotEmpty();

            RuleFor(f => f.File)
                .NotNull();
        }
    }
}
