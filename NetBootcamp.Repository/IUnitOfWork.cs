namespace NetBootcamp.Repository
{
    public interface IUnitOfWork
    {
        // db de etkilenen satır sayısını döner
        int Commit();
        Task<int> CommitAsync();
    }
}
