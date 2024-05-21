namespace NetBootcamp.API.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public int Commit()
        {
            return context.SaveChanges();   // Geriye etki ettiği işlem sayısını döndürür. Ör. 3 update insert yapıldıysa geriye 4 döner.
        }

        public Task<int> CommitAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
