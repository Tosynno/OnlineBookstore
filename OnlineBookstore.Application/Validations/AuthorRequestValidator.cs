using FluentValidation;
using OnlineBookstore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Application.Validations
{
    public  class AuthorRequestValidator :AbstractValidator<AuthorRequest>
    {
        public AuthorRequestValidator()
        {
            RuleFor(x => x.AuthorName).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.AuthorInfo).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Imagebase64string).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}
