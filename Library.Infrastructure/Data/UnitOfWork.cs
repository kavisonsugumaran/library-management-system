using Library.Application.Abstractions;

namespace Library.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _ctx;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext ctx) => _ctx = ctx;

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repo)) return (IRepository<T>)repo!;
            var instance = new Repository<T>(_ctx);
            _repositories[typeof(T)] = instance;
            return instance;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _ctx.SaveChangesAsync(cancellationToken);

        public async ValueTask DisposeAsync() => await _ctx.DisposeAsync();
    }
}
