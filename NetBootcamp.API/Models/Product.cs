namespace NetBootcamp.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;    // null değer almayacağını belirtmiş olduk. Yine de null değer alabilir fakat uyarı niteliğinde.
        public decimal Price { get; set; }
        public DateTime Created { get; set; }
        public string Barcode { get; init; } = default!;
    }
}
