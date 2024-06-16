namespace NetBootcamp.Repository.Products
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;    // null değer almayacağını belirtmiş olduk. Yine de null değer alabilir fakat uyarı niteliğinde.
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public string Barcode { get; init; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
    }
}
