using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetBootcamp.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<IImmutableList<T>> GetAll();
        Task<T> GetById(int id);
        Task<IImmutableList<T>> Get(Expression<Func<T,bool>> expression);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
