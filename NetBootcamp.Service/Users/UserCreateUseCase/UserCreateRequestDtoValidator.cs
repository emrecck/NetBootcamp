using FluentValidation;

namespace NetBootcamp.Service.Users.UserCreateUseCase
{
    public class UserCreateRequestDtoValidator : AbstractValidator<UserCreateRequestDto>
    {
        public UserCreateRequestDtoValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Email is required").EmailAddress().NotEmpty();
            RuleFor(x => x.PhoneNumber).NotNull().Length(11).WithState(x => x.PhoneNumber.StartsWith('0'));
            RuleFor(x => x.Name).NotNull().WithName("Name is required");
            RuleFor(x => x.Surname).NotNull().WithName("Surname is required");
        }
    }
}
