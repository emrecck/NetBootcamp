namespace NetBootcamp.Service.Products.Helpers
{
    public class PriceCalculator
    {
        public static decimal CalculateKdv(decimal price, decimal tax) => price * tax;
    }
}
