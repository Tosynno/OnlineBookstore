using FluentValidation;
using OnlineBookstore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Validations
{
    public class BookRequestValidator : AbstractValidator<BookRequest>
    {
        public BookRequestValidator() {
            RuleFor(x => x.BookName).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Imagestring).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("Enter a valid value");
        }
    }
}
