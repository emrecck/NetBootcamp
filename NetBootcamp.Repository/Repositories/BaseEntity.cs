namespace NetBootcamp.Repository.Repositories
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; } = default!;
    }
}
