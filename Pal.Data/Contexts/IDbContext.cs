using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Pal.Data.Contexts
{
    public interface IDbContext
    {

        public bool IsEnable_SoftDelete();
        public DbSet<TEntity> Set<TEntity>() where TEntity : class;
        public EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;

        public void Dispose();
        public ValueTask DisposeAsync();
        public IDbContextTransaction BeginTransaction();

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
