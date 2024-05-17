using FluentValidation;
using FluentValidation.Results;

namespace NetBootcamp.API.Products.DTOs.ProductCreateUseCase
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequestDto>
    {
        public ProductCreateRequestValidator(IProductRepository productRepository)
        {
            RuleFor(x=>x.Name)
                .NotEmpty().WithMessage("Name is required")
                .NotNull().WithMessage("Name cannot be null")
                .Length(5,10).WithMessage("Name length is must be between 5-10.")
                .Must(productName => ExistProductName(productRepository, productName)).WithMessage("Productname must be unique");

            RuleFor(x => x.Price).InclusiveBetween(1, 1000).WithMessage("Price must be between 1-1000");
            /*RuleFor(x => x.IdentityNo).Length(11).WithMessage("Identity no must be 11 characters.").Must(CheckIdentityNo).WithMessage("Identity no is wrong");*/  // Must ile custom validation yazılabilir.
            RuleFor(x => x).Must(CheckDtoProps).WithMessage("Check the props"); // check all propertys
        }

        public bool ExistProductName(IProductRepository productRepository, string name)
        {
            var hasProduct = productRepository.IsExist(name);

            return !hasProduct; // if there is product with this name then return False because this this validation fail.
        }

        // Custom validation for Identity no
        public static bool CheckIdentityNo(string IdentityNo) 
        {
            return true;
        }

        public static bool CheckDtoProps(ProductCreateRequestDto request)
        {
            return true;
        }
    }
}
