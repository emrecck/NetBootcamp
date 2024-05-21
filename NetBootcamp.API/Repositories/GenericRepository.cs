
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NetBootcamp.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<int>
    {
        public DbSet<T> DbSet { get; set; }
        protected AppDbContext Context { get; set; }
        public GenericRepository(AppDbContext context)
        {
            DbSet = context.Set<T>();
            Context = context;
        }
        public async Task<T> CreateAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            DbSet.Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var list = await DbSet.ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<IReadOnlyList<T>> GetAllWithExpressionAsync(Expression<Func<T, bool>> expression)
        {
            var list = await DbSet.Where(expression).ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await DbSet.FindAsync(id);
            return result!;
        }

        public async Task<IReadOnlyList<T>> GetByPagingAsync(int page, int pageSize)
        {
            var list = await DbSet.Skip((page - 1) * pageSize).Take(page).ToListAsync();
            return list.AsReadOnly();
        }

        public Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task<bool> IsExist(int id)
        {
            return DbSet.AnyAsync(x => x.Id == id);
        }
    }
}
