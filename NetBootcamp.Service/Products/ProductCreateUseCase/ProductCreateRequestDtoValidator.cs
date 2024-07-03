using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBootcamp.Service.Products.ProductCreateUseCase
{
    public class ProductCreateRequestDtoValidator : AbstractValidator<ProductCreateRequestDto>
    {
        public ProductCreateRequestDtoValidator()
        {
            RuleFor(x=>x.Price).InclusiveBetween(1m,1000m).WithMessage("Price must be between 1-1000").NotEmpty();
            RuleFor(x=>x.Name).NotNull().WithMessage("Name is required");
        }
    }
}
