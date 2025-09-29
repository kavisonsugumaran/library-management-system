using Library.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _ctx;
        private readonly DbSet<T> _db;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
            _db = ctx.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db;
            foreach (var include in includes) query = query.Include(include);
            return await query.SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _db;
            if (filter != null) query = query.Where(filter);
            foreach (var include in includes) query = query.Include(include);
            if (orderBy != null) query = orderBy(query);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(T entity) => await _db.AddAsync(entity);
        public void Update(T entity) => _db.Update(entity);
        public void Remove(T entity) => _db.Remove(entity);
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _db.AnyAsync(predicate);
    }
}
