using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetBootcamp.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetBootcamp.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity, new()
    {
        public DbSet<T> Entity { get; set; }
        public EFDbContext DbContext { get; set; }

        public GenericRepository(EFDbContext dbContext)
        {
            Entity = dbContext.Set<T>();
            DbContext = dbContext;
        }

        public async Task<T> Add(T entity)
        {
            await Entity.AddAsync(entity);
            return entity;
        }
        public Task Update(T entity)
        {
            Entity.Update(entity);
            return Task.CompletedTask;
        }
        public Task Delete(T entity)
        {
            Entity.Remove(entity);
            return Task.CompletedTask;
        }
        public async Task<IImmutableList<T>> GetAll()
        {
            var list = await Entity.ToListAsync();
            return list.ToImmutableList();
        }
        public async Task<T> GetById(int id)
        {
            T? entity = await Entity.FindAsync(id);
            return entity!;
        }
        public async Task<IImmutableList<T>> Get(Expression<Func<T, bool>> expression)
        {
            var list = await Entity.Where(expression).ToListAsync();
            return list.ToImmutableList();
        }
    }
}
