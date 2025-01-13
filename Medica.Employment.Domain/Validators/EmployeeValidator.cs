using Medica.Employment.Domain.Entities;
using FluentValidation;

namespace Medica.Employment.Domain.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid Email Address");

            RuleFor(e => e.Telephone)
                .Matches("^\\d+$").When(e => !string.IsNullOrEmpty(e.Telephone))
                .WithMessage("Telephone must contain only digits");

            RuleFor(e => e.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past");

            RuleFor(e => e.Address1)
                .NotEmpty().WithMessage("Address 1 is required");

            RuleFor(e => e.Address2)
                .NotEmpty().WithMessage("Address 2 is required");

            RuleFor(e => e.LineManager)
               .NotEmpty().WithMessage("LineManager is required");

            RuleFor(e => e.Postcode)
                .NotEmpty().WithMessage("Postcode is required");

            RuleFor(e => e.JobTitle)
                .NotEmpty().WithMessage("Job Title is required");

            RuleFor(e => e.StartDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Start Date is required");
        }
    }
}
