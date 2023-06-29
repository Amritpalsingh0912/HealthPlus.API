using FluentValidation;
using HealthPlus.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPlus.Domain.Validators
{
    public class PatientInfoValidators: AbstractValidator<PatientInfoDTO>
    {
        public PatientInfoValidators()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required")
                     .EmailAddress().WithMessage("A valid email is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required").Length(10);
            RuleFor(x => x.Gender).IsInEnum();


        }
    }
}
