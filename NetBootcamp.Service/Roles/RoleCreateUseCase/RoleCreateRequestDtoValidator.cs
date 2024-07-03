using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBootcamp.Service.Roles.RoleCreateUseCase
{
    public class RoleCreateRequestDtoValidator : AbstractValidator<RoleCreateRequestDto>
    {
        public RoleCreateRequestDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name is required");
        }
    }
}
