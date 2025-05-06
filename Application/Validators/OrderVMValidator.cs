using Domain.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Application.Validators
{
    public class OrderVMValidator : AbstractValidator<OrderVM>
    {
        public OrderVMValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100);

            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("Shipping address is required.")
                .MaximumLength(200);

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than 0.");            

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("At least one order item is required.");
        }

        private bool BeAValidEnum<TEnum>(string value)
        {
            return Enum.TryParse(typeof(TEnum), value, true, out _);
        }
    }

}
