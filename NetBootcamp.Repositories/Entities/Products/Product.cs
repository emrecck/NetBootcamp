namespace NetBootcamp.Repositories.Entities.Products
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = default!;    // null değer almayacağını belirtmiş olduk. Yine de null değer alabilir fakat uyarı niteliğinde.
        public decimal Price { get; set; }
        public string Barcode { get; init; } = default!;
    }
}
