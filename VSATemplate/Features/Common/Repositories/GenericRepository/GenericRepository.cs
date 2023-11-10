using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using VSATemplate.Features.Common.Entities.Base;
using VSATemplate.Features.Common.Data;
using VSATemplate.Features.Common.Repositories.GenericRepository.Base;
using IdentityModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace VSATemplate.Features.Common.Repositories.GenericRepository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DataContext _dataContext;
        protected DbSet<T> Entities => _dataContext.Set<T>();

        protected GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<T> insertedValue = await _dataContext.Set<T>().AddAsync(entity, cancellationToken);
            return insertedValue.Entity;
        }

        public async Task<T?> GetByIdAsync(CancellationToken cancellationToken = default, params object[] keys)
        {
            var entity = await _dataContext.Set<T>().FindAsync(cancellationToken, keys);

            if (entity == null) return null;
            else return entity;
        }

        public virtual async Task<bool> IsExistsAsync(Expression<Func<T, bool>> querySelector, CancellationToken cancellationToken = default)
        {
            return await GetAsync(querySelector, cancellationToken) is not null;
        }

        public virtual Task<T?> GetAsync(Expression<Func<T, bool>> querySelector, CancellationToken cancellationToken = default)
        {
            return _dataContext.Set<T>().FirstOrDefaultAsync(querySelector, cancellationToken);
        }

        public void UpdateAsync(T entity, CancellationToken cancellationToken = default)
            => _dataContext.Set<T>().Update(entity);

        public async Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T? entity = await GetByIdAsync(cancellationToken, id);

            if (entity is null)
                return false;

            _dataContext.Set<T>().Remove(entity);
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T? entity = await GetByIdAsync(cancellationToken, id);

            if (entity is null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedTimeUtc = DateTime.UtcNow;

            _dataContext.Set<T>().Update(entity);
            return true;
        }

        public async Task<bool?> WasSoftDeletedAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T? entity = await GetByIdAsync(cancellationToken, id);

            if (entity is null)
                return null;
            else return entity.IsDeleted; 
        }

        public IQueryable<T> GetAllAsync(CancellationToken cancellationToken = default)
            => _dataContext.Set<T>();        

        public virtual IQueryable<T> ExecuteQuery([Optional] Expression<Func<T, bool>>? querySelector)
        {
            return _dataContext.Set<T>().Where(querySelector ?? (x => true));
        }
    }
}
