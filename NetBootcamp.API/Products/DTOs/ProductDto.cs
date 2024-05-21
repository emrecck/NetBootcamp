namespace NetBootcamp.API.Products.DTOs
{
    public record ProductDto(int Id, string Name, decimal Price, string Created, string Barcode, int Stock);
    //{
    //    public int Id { get; init; }
    //    public string Name { get; init; } = default!;    // null değer almayacağını belirtmiş olduk. Yine de null değer alabilir fakat uyarı niteliğinde.
    //    public decimal Price { get; init; }
    //    public string Created { get; init; } = default!;
    //}
}
