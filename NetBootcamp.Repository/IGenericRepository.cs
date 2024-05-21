using NetBootcamp.Repository.Products;
using System.Linq.Expressions;

namespace NetBootcamp.Repository
{
    public interface IGenericRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetByPagingAsync(int page, int pageSize);
        Task<IReadOnlyList<T>> GetAllWithExpressionAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> IsExist(int id);
    }
}
