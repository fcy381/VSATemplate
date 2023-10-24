using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using VSATemplate.Features.Common.Entities.Base;
using VSATemplate.Features.Common.Data;
using VSATemplate.Features.Common.Repositories.GenericRepository.Base;

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

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> insertedValue = await _dataContext.Set<T>().AddAsync(entity);
            return insertedValue.Entity;
        }

        public async Task<T?> GetById(params object[] keys)
        {
            var entity = await _dataContext.Set<T>().FindAsync(keys);

            if (entity == null) return null;
            else return entity;
        }

        public async Task<bool?> WasSoftDeleted(Guid id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return null;
            else return entity.IsDeleted; 
        }

        public IQueryable<T> GetAll()
            => _dataContext.Set<T>();

        public IQueryable<T> GetAllEvenThoseSoftDeleted()
            => _dataContext.Set<T>();

        public void Update(T entity)
            => _dataContext.Set<T>().Update(entity);

        public async Task<bool> HardDelete(Guid id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            _dataContext.Set<T>().Remove(entity);
            return true;
        }

        public async Task<bool> SoftDelete(Guid id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedTimeUtc = DateTime.UtcNow;

            _dataContext.Set<T>().Update(entity);

            return true;
        }

        public async Task<bool> Rescue(Guid id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            entity.IsDeleted = false;
            entity.DeletedTimeUtc = DateTime.MinValue;

            return true;
        }
    }
}
