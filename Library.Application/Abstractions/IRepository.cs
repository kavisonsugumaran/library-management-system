using System.Linq.Expressions;

namespace Library.Application.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity);

        void Update(T entity);

        void Remove(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
