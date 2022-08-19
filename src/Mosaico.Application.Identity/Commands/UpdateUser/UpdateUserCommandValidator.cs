using System;
using FluentValidation;

namespace Mosaico.Application.Identity.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
            
            RuleFor(c => c.FirstName)
                .NotEmpty().WithErrorCode("INVALID_FIRST_NAME")
                .Length(2, 50).WithErrorCode("INVALID_FIRST_NAME")
                .Matches(Base.Constants.RegularExpressions.BasicInputString).WithErrorCode("INVALID_FIRST_NAME");
            
            RuleFor(c => c.LastName).NotEmpty().WithErrorCode("INVALID_LAST_NAME")
                .Length(2, 50).WithErrorCode("INVALID_LAST_NAME")
                .Matches(Base.Constants.RegularExpressions.BasicInputString).WithErrorCode("INVALID_LAST_NAME");
            
            When(t => !string.IsNullOrEmpty(t.Country), () =>
            {
                RuleFor(t => t.Country).Length(2, 5).WithErrorCode("INVALID_COUNTRY")
                    .Matches(Base.Constants.RegularExpressions.BasicInputString).WithErrorCode("INVALID_COUNTRY");
            });
            When(t => !string.IsNullOrEmpty(t.City), () =>
            {
                RuleFor(t => t.City).Length(2, 50).WithErrorCode("INVALID_CITY")
                    .Matches(Base.Constants.RegularExpressions.BasicInputString).WithErrorCode("INVALID_CITY");
            });
            When(t => !string.IsNullOrEmpty(t.Street), () =>
            {
                RuleFor(t => t.Street).Length(2, 250).WithErrorCode("INVALID_STREET")
                    .Matches(Base.Constants.RegularExpressions.BasicInputString).WithErrorCode("INVALID_STREET");
            });
            When(t => !string.IsNullOrEmpty(t.Timezone), () =>
            {
                RuleFor(t => t.Timezone).Length(1, 25).WithErrorCode("INVALID_TIMEZONE")
                    .Matches(Base.Constants.RegularExpressions.BasicInputString).WithErrorCode("INVALID_TIMEZONE");
            });
            When(t => !string.IsNullOrEmpty(t.PostalCode), () =>
            {
                RuleFor(t => t.PostalCode).Length(1, 25).WithErrorCode("INVALID_POSTAL_CODE")
                    .Matches(Base.Constants.RegularExpressions.BasicInputString).WithErrorCode("INVALID_POSTAL_CODE");
            });
            When(t => t.Dob.HasValue, () =>
            {
                RuleFor(t => t.Dob)
                    .Must(time => time != default).WithErrorCode("INVALID_DATE_OF_BIRTH")
                    .Must(time =>
                        time >= DateTimeOffset.UtcNow.AddYears(-100) && time < DateTimeOffset.UtcNow).WithErrorCode("INVALID_DATE_OF_BIRTH");
            });
        }
    }
}