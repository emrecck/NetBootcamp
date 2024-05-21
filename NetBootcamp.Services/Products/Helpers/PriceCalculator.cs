namespace NetBootcamp.Services.Products.Helpers
{
    public class PriceCalculator
    {
        public decimal CalculateKdv(decimal price, decimal tax) => price * tax;
    }
}
