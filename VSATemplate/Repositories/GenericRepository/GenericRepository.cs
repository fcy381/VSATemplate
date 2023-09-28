using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using VSATemplate.Entities.Base;
using VSATemplate.Repositories.GenericRepository.Base;
using VSATemplate.Data;

namespace VSATemplate.Repositories.GenericRepository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DataContext _dataContext;
        protected DbSet<T> Entities => _dataContext.Set<T>();

        public GenericRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> insertedValue = await _dataContext.Set<T>().AddAsync(entity);
            return insertedValue.Entity;
        }

        public async Task<T?> GetById(int id)
            => await _dataContext.Set<T>()
                .Where(x => x.IsDeleted == false && x.Id == id)
                .FirstOrDefaultAsync();

        public async Task<bool?> WasSoftDeleted(T entity)
        {
            if (entity != null) { return entity.IsDeleted; }
            else { return null; };
        }

        public IQueryable<T> GetAll()
            => _dataContext.Set<T>().Where(x => x.IsDeleted == false);

        public IQueryable<T> GetAllEvenThoseSoftDeleted()
            => _dataContext.Set<T>().Where(x => x.IsDeleted == false);

        public void Update(T entity)
            => _dataContext.Set<T>().Update(entity);

        public async Task<bool> HardDelete(int id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            _dataContext.Set<T>().Remove(entity);
            return true;
        }

        public async Task<bool> SoftDelete(int id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedTimeUtc = DateTime.UtcNow;

            _dataContext.Set<T>().Update(entity);

            return true;
        }
    }
}
