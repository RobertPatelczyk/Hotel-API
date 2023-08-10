using FluentValidation;
using HotelReview.Entities;
using HotelReview.Models.User;
using System.Linq;

namespace HotelReview.Models.Validators
{
    public class RegisterUserDtoValidator :AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(HotelDbContext dbContext)
        {
            RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress();
            RuleFor(x => x.Password)
                .MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });
        }
    }
}
