namespace NetBootcamp.API.Products.Helpers
{
    public class PriceCalculator
    {
        public decimal CalculateKdv(decimal price, decimal tax) => price * tax;
    }
}
