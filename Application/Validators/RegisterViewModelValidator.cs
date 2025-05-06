﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Auth;

namespace Application.Validators
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterVM>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .Length(5, 20).WithMessage("Username must be between 5 and 20 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");
        }
    }
}
